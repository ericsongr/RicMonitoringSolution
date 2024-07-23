using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicCommon.Enumeration;
using RicEntityFramework.RicXplorer.Interfaces;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RicXplorer;
using RicModel.RicXplorer.Dtos;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Enumeration;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.Infrastructure.Helpers;
using RicMonitoringAPI.RicXplorer.ViewModels;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/guest-booking")]
    [ApiController]
    public class GuestBookingController : ControllerBase
    {
        private readonly IGuestBookingDetailRepository _guestBookingDetailRepository;
        private readonly IGuestCheckListRepository _guestCheckListRepository;
        private readonly ILookupTypeRepository _lookupTypeRepository;
        private readonly ILookupTypeItemRepository _lookupTypeItemRepository;
        private readonly IAccountProductRepository _accountProductRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IBookingTypeRepository _bookingTypeRepository;
        private readonly IMapper _mapper;

        public GuestBookingController(
            IGuestBookingDetailRepository guestBookingDetailRepository,
            IGuestCheckListRepository guestCheckListRepository,
            ILookupTypeRepository lookupTypeRepository,
            ILookupTypeItemRepository lookupTypeItemRepository,
            IAccountProductRepository accountProductRepository,
            ISettingRepository settingRepository,
            IBookingTypeRepository bookingTypeRepository,
            IMapper mapper)
        {
            _guestBookingDetailRepository = guestBookingDetailRepository ?? throw new ArgumentNullException(nameof(guestBookingDetailRepository));
            _guestCheckListRepository = guestCheckListRepository ?? throw new ArgumentNullException(nameof(guestCheckListRepository));
            _lookupTypeRepository = lookupTypeRepository ?? throw new ArgumentNullException(nameof(lookupTypeRepository));
            _lookupTypeItemRepository = lookupTypeItemRepository ?? throw new ArgumentNullException(nameof(lookupTypeItemRepository));
            _accountProductRepository = accountProductRepository ?? throw new ArgumentNullException(nameof(accountProductRepository));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
            _bookingTypeRepository = bookingTypeRepository ?? throw new ArgumentNullException(nameof(bookingTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [AllowAnonymous]
        [HttpGet("guests-booked-schedules")]
        public IActionResult GuestsBookedSchedules(string startDate, string endDate, int bookingType)
        {
            DateTime.TryParse(startDate, out DateTime arrivalDate);
            DateTime.TryParse(endDate, out DateTime departureDate);


            var guests = _guestBookingDetailRepository.Find(arrivalDate, departureDate, bookingType)
                .GroupBy(g => new
                {
                    g.DateBooked,
                    g.GuestBookingDetail.BookingTypeModel.AccountProduct.MaximumLevelQuantity
                })
                .Select(p => new GuestBookedSchedule
                {
                    BookedDate = p.Key.DateBooked,
                    TotalGuestsBooked = p.Count(),
                    IsFullyBooked = p.Count() >= p.Key.MaximumLevelQuantity
                });

            return Ok(new BaseRestApiModel
            {
                Payload = guests,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }


        [AllowAnonymous]
        [HttpGet("guests-confirmed-bookings")]
        public IActionResult GuestsConfirmedBookings(string startDate, string endDate, int bookingType)
        {
            DateTime.TryParse(startDate, out DateTime arrivalDate);
            DateTime.TryParse(endDate, out DateTime departureDate);

            var dataGuests = _guestBookingDetailRepository.FindBookings(arrivalDate, departureDate, bookingType);
            var guests = _mapper.Map<IEnumerable<GuestBookingListDto>>(dataGuests).ToList();

            return Ok(new BaseRestApiModel
            {
                Payload = guests,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpGet("detail/{id:int}")]
        public IActionResult GuestsBookingDetail(int id)
        {

            var data = _guestBookingDetailRepository.FindBookingByIdv2(id);
            var guest = _mapper.Map<GuestBookingDetailDto>(data);

            guest.RoomOptions =_lookupTypeRepository
                .FindBy(o => guest.BookingOptionIds.Contains(o.Id))
                .Select(o => new LookupTypeOnlyDto
                {
                    Value  = o.Id.ToString(),
                    Text = o.Name
                }).ToList();

            if (data.BookingType == 3) //if booking type selected is family then family room id should be default id
            {
                guest.RoomId = int.Parse(guest.RoomOptions[0].Value);
            }

            return Ok(new BaseRestApiModel
            {
                Payload = guest,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [AllowAnonymous]
        [HttpGet("cost/{bookingId:int}")]
        public IActionResult BookingCost(int bookingId, string startDate, string endDate)
        {
            DateTime.TryParse(startDate, out DateTime checkInDate);
            DateTime.TryParse(endDate, out DateTime checkOutDate);

            int bookingDays = (checkOutDate - checkInDate).Days;

            var productItem = _accountProductRepository.FindBy(o => o.Id == bookingId).FirstOrDefault();
            var settingVat = _settingRepository.Get(SettingNameEnum.VatPH);
            if (productItem == null || settingVat == null)
            {
                return NotFound();
            }

            var costLabel = productItem.Id == 3 ? productItem.Name : $"{productItem.Name} bed";
            var vat = decimal.Parse(settingVat.Value.Replace("%", "")) / 100;
            var vatCost = (productItem.OnlinePrice * bookingDays) * vat;
            var bookingCost = (productItem.OnlinePrice * bookingDays) - vatCost;
            
            var model = new 
            {
                period = $"{checkInDate.ToString("dd-MMM-yyy")} to {checkOutDate.ToString("dd-MMM-yyy")}",
                bookingDays = $"{bookingDays} {(bookingDays == 1 ? "day" : "days")}",
                costLabel,
                cost = productItem.OnlinePrice.ToString("#,###.00"),
                vatCost = vatCost.ToString("#,###.00"),
                totalCostExcludedVAT = bookingCost.ToString("#,###.00"),
                totalCostIncludedVAT = (bookingCost + vatCost).ToString("#,###.00"),
            };

            return Ok(new BaseRestApiModel
            {
                Payload = model,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [AllowAnonymous]
        [HttpPost("book", Name = "book")]
        public IActionResult Book(CreateGuestBookingDto model)
        {
            bool isManyGuests = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(HandleApi.Exception("Error when booking.", HttpStatusCode.NotFound));
                }
                else
                {
                    //automatic assign backpacker/couple bed or room
                    var bookingType = _bookingTypeRepository.FindBy(o => o.Id == model.BookingType).FirstOrDefault();
                    if (bookingType == null)
                        return NotFound("Booking Type not found");

                    var linkRoomIdToList = bookingType.LinkRooms
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse).ToList();

                    //get all beds and rooms
                    var bedOrRoomIds = _lookupTypeItemRepository
                        .FindBy(o => linkRoomIdToList.Contains(o.LookupTypeId))
                        .Select(o => o.Id)
                        .ToList();
                    if (bedOrRoomIds == null)
                        return NotFound("guest beds or room not found from look up type item.");

                    var existingData = _guestBookingDetailRepository
                        .FindBy(o =>
                            o.BookingType == model.BookingType && bedOrRoomIds.Contains(o.RoomOrBedId ?? 0) &&
                            o.CheckedOutDateTime == null)
                        .Select(o => o.RoomOrBedId ?? 0).ToList();

                    var availableBedOrRooms = bedOrRoomIds.Where(bedOrRoomId => !existingData.Contains(bedOrRoomId)).ToList();
                    if (!availableBedOrRooms.Any())
                        return NotFound("No available bed or room.");

                    //get available bed or room and then assign to confirmed guest.
                    int availableBedOrRoomId = availableBedOrRooms[0]; //get one


                    //save both parent and children guests details
                    var guestBookingDetail = _mapper.Map<GuestBookingDetail>(model);
                    guestBookingDetail.AccountId = 1; //TODO: create account
                    guestBookingDetail.RoomOrBedId = availableBedOrRoomId;

                    guestBookingDetail.GuestBookingDates = new List<GuestBookingDate>();
                    for (DateTime startDate = guestBookingDetail.ArrivalDate; startDate <= guestBookingDetail.DepartureDate; startDate = startDate.AddDays(1))
                    {
                        guestBookingDetail.GuestBookingDates.Add(new GuestBookingDate
                        {
                            DateBooked = startDate
                        });
                    }

                    _guestBookingDetailRepository.Add(guestBookingDetail);
                    _guestBookingDetailRepository.Commit();
                    

                    isManyGuests = guestBookingDetail.GuestBookings.Count > 1;

                    return Ok(new BaseRestApiModel
                    {
                        Payload = $"{(isManyGuests ? "Guests" : "Guest")} has been booked.",
                        Errors = new List<BaseError>(),
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }
        
        [HttpPost("check-in", Name = "CheckIn")]
        public IActionResult CheckIn(GuestCheckInCheckOut model)
        {
            try
            {
                var guestBooking = _guestBookingDetailRepository.FindBookingById(model.Id);
                if (guestBooking != null)
                {
                    guestBooking.CheckedInDateTime = DateTime.Now;
                    guestBooking.CheckedInBy = model.Username;
                }

                _guestBookingDetailRepository.Update(guestBooking);
                _guestBookingDetailRepository.Commit();

                //save check list
                model.GuestCheckList.ForEach(item =>
                {
                    var itemModel = _guestCheckListRepository.GetSingleAsync(o =>
                            o.GuestBookingDetailId == model.Id && o.CheckListId == item.CheckListId)
                        .GetAwaiter().GetResult();
                    if (itemModel == null)
                    {
                        _guestCheckListRepository.Add(new GuestCheckList
                        {
                            GuestBookingDetailId = model.Id,
                            CheckListId = item.CheckListId,
                            IsChecked = item.IsChecked,
                            Notes = item.Notes
                        });
                    }
                    else
                    {
                        itemModel.IsChecked = item.IsChecked;
                        itemModel.Notes = item.Notes;
                        _guestCheckListRepository.Update(itemModel);
                    }

                });

                _guestCheckListRepository.Commit();

                return Ok(new BaseRestApiModel
                {
                    Payload = new
                    {
                        checkedInDateTimeString = guestBooking.CheckedInDateTime?.ToString("dd-MMM-yyyy hh:mm tt"),
                        message = "Guest(s) has been checked-in"
                    },
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost("check-out", Name = "CheckOut")]
        public IActionResult CheckOut(GuestCheckInCheckOut model)
        {
            try
            {
                var guestBooking = _guestBookingDetailRepository.FindBookingById(model.Id);
                if (guestBooking != null)
                {
                    guestBooking.CheckedOutDateTime = DateTime.Now;
                    guestBooking.CheckedOutBy = model.Username;
                }

                _guestBookingDetailRepository.Update(guestBooking);
                _guestBookingDetailRepository.Commit();

                //save check list
                model.GuestCheckList.ForEach(item =>
                {
                    var itemModel = _guestCheckListRepository.GetSingleAsync(o =>
                            o.GuestBookingDetailId == model.Id && o.CheckListId == item.CheckListId)
                        .GetAwaiter().GetResult();
                    if (itemModel == null)
                    {
                        _guestCheckListRepository.Add(new GuestCheckList
                        {
                            GuestBookingDetailId = model.Id,
                            CheckListId = item.CheckListId,
                            IsChecked = item.IsChecked,
                            Notes = item.Notes
                        });
                    }
                    else
                    {
                        itemModel.IsChecked = item.IsChecked;
                        itemModel.Notes = item.Notes;
                        _guestCheckListRepository.Update(itemModel);
                    }

                });

                _guestCheckListRepository.Commit();

                return Ok(new BaseRestApiModel
                {
                    Payload = new
                    {
                        checkedOutDateTimeString = guestBooking.CheckedInDateTime?.ToString("dd-MMM-yyyy hh:mm tt"),
                        message = "Guest(s) has been checked-out"
                    },
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost("save-room-or-bed", Name = "SaveRoomOrBed")]
        public IActionResult SaveRoomOrBed(GuestRoomOrBedDto model)
        {
            try
            {
                var guestBooking = _guestBookingDetailRepository.FindBookingById(model.GuestBookingDetailId);
                if (guestBooking != null)
                {
                    guestBooking.RoomOrBedId = model.RoomOrBedId == 0 ? null : model.RoomOrBedId;
                }
                _guestBookingDetailRepository.Update(guestBooking);
                _guestBookingDetailRepository.Commit();

                var roomInfo = _guestBookingDetailRepository.FindBookingByIdv2(model.GuestBookingDetailId);
                if (roomInfo == null)
                    throw new ArgumentNullException("Not found");

                var roomName = roomInfo.RoomOrBed.LookupTypes.Name;
                var roomOrBedName = $"{roomInfo.RoomOrBed.Notes} [{roomInfo.RoomOrBed.Description}]";

                string prefix = guestBooking.BookingType == 3 ? "room" : "bed";

                return Ok(new BaseRestApiModel
                {
                    Payload = new
                    {
                        message = "Assigned " + prefix + " has been saved.",
                        roomName,
                        roomOrBedName,
                    },
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }

        #region API Test Data

        [AllowAnonymous]
        [HttpPost("book-single-test", Name = "BookSingleTest")]
        public IActionResult BookSingleTest()
        {
            try
            {
                var faker = new Faker();
                var arrivalDate = new DateTime(2024, 7, 11);
                var departureDate = new DateTime(2024, 7, 13);
                var guestBookings = new List<GuestBookingDto>
                {
                    new GuestBookingDto
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Male),
                        LastName = faker.Name.LastName(Name.Gender.Male),
                        Gender = "Male",
                        Ages = 1
                    }
                };

                var guestBooking = new CreateGuestBookingDto
                {
                    ArrivalDate = arrivalDate,
                    DepartureDate = departureDate,
                    BookingType = (int)BookingTypeEnum.Backpacker,
                    Contact = "+069999999999",
                    ContactPerson = $"{guestBookings[0].FirstName} {guestBookings[0].LastName}",
                    Country = "Philippines",
                    LanguagesSpoken = "English",
                    Email = "english@yahoo.com",
                    LeaveMessage = "Test leave message",
                    SelectedPayment = "GCASH",
                    GuestBookings = guestBookings
                };

                Book(guestBooking);

                return Ok(new BaseRestApiModel
                {
                    Payload = "Booking backpacker test data has been successful",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }

        [AllowAnonymous]
        [HttpPost("book-couple-test", Name = "BookCoupleTest")]
        public IActionResult BookCoupleTest()
        {
            try
            {
                var faker = new Faker();
                var arrivalDate = new DateTime(2024, 7, 11);
                var departureDate = new DateTime(2024, 7, 13);
                var guestBookings = new List<GuestBookingDto>
                {
                    new GuestBookingDto
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Male),
                        LastName = faker.Name.LastName(Name.Gender.Male),
                        Gender = "Male",
                        Ages = 1
                    },
                    new GuestBookingDto
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Female),
                        LastName = faker.Name.LastName(Name.Gender.Female),
                        Gender = "Male",
                        Ages = 1
                    }
                };

                var guestBooking = new CreateGuestBookingDto
                {
                    ArrivalDate = arrivalDate,
                    DepartureDate = departureDate,
                    BookingType = (int)BookingTypeEnum.CoupleBackpackers,
                    Contact = "+069999999999",
                    ContactPerson = $"{guestBookings[0].FirstName} {guestBookings[0].LastName}",
                    Country = "Philippines",
                    LanguagesSpoken = "English",
                    Email = "english@yahoo.com",
                    LeaveMessage = "Test leave message",
                    SelectedPayment = "GCASH",
                    GuestBookings = guestBookings
                };

                Book(guestBooking);

                return Ok(new BaseRestApiModel
                {
                    Payload = "Booking couple test data has been successful",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }

        [AllowAnonymous]
        [HttpPost("book-family-test", Name = "BookFamilyTest")]
        public IActionResult BookFamilyTest()
        {
            try
            {
                var faker = new Faker();
                var arrivalDate = new DateTime(2024, 7, 11);
                var departureDate = new DateTime(2024, 7, 13);
                var guestBookings = new List<GuestBookingDto>
                {
                    new GuestBookingDto
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Male),
                        LastName = faker.Name.LastName(Name.Gender.Male),
                        Gender = "Male",
                        Ages = 1
                    },
                    new GuestBookingDto
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Male),
                        LastName = faker.Name.LastName(Name.Gender.Male),
                        Gender = "Male",
                        Ages = 14
                    },
                    new GuestBookingDto
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Female),
                        LastName = faker.Name.LastName(Name.Gender.Female),
                        Gender = "Female",
                        Ages = 56
                    },
                    new GuestBookingDto
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Female),
                        LastName = faker.Name.LastName(Name.Gender.Female),
                        Gender = "Female",
                        Ages = 17
                    }
                };

                var guestBooking = new CreateGuestBookingDto
                {
                    ArrivalDate = arrivalDate,
                    DepartureDate = departureDate,
                    BookingType = (int)BookingTypeEnum.FamilyRoom,
                    Contact = "+069999999999",
                    ContactPerson = $"{guestBookings[0].FirstName} {guestBookings[0].LastName}",
                    Country = "Philippines",
                    LanguagesSpoken = "English",
                    Email = "english@yahoo.com",
                    LeaveMessage = "Test leave message",
                    SelectedPayment = "GCASH",
                    GuestBookings = guestBookings
                };

                Book(guestBooking);

                return Ok(new BaseRestApiModel
                {
                    Payload = "Booking family test data has been successful",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException?.Message, HttpStatusCode.InternalServerError));
            }
        }

        #endregion

    }
}