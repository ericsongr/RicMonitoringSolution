using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicCommon.Infrastructure.Extensions;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;
using RicModel.RicXplorer.Dtos;
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
        private readonly IMapper _mapper;

        public GuestBookingController(
            IGuestBookingDetailRepository guestBookingDetailRepository,
            IMapper mapper)
        {
            _guestBookingDetailRepository = guestBookingDetailRepository ?? throw new ArgumentNullException(nameof(guestBookingDetailRepository));
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
            var guests = _mapper.Map<IEnumerable<GuestBookingDetailDto>>(dataGuests).ToList();

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

            var data = _guestBookingDetailRepository.FindBookingById(id);
            var guests = _mapper.Map<GuestBookingDetailDto>(data);

            return Ok(new BaseRestApiModel
            {
                Payload = guests,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [AllowAnonymous]
        [HttpPost("book", Name = "book")]
        public IActionResult Book(GuestBookingDetailDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(HandleApi.Exception("Error when booking.", HttpStatusCode.NotFound));
                }
                else
                {
                    //save both parent and children guests details
                    var guestBookingDetail = _mapper.Map<GuestBookingDetail>(model);
                    guestBookingDetail.AccountId = 1; //TODO: create account
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

                    return Ok(new BaseRestApiModel
                    {
                        Payload = "Booking has been successful",
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

    }
}