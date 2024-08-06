using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicEntityFramework.RicXplorer.Repositories;
using RicModel.CostMonitoring.Dtos;
using RicModel.RicXplorer;
using RicModel.RicXplorer.Dtos;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.RicXplorer.ViewModels;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/guest-booking-types")]
    [ApiController]
    public class GuestBookingTypeController : ControllerBase
    {
        private readonly ILookupTypeItemRepository _lookupTypeItemRepository;
        private readonly IBookingTypeInclusionRepository _bookingTypeInclusionRepository;
        private readonly IBookingTypeRepository _bookingTypeRepository;
        private readonly IAccountProductRepository _accountProductRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IMapper _mapper;
        private readonly int _bookingTypeInclusionId = 3; //Booking Type Inclusions from look up
        public GuestBookingTypeController(
            ILookupTypeItemRepository lookupTypeItemRepository,
            IBookingTypeInclusionRepository bookingTypeInclusionRepository,
            IBookingTypeRepository bookingTypeRepository,
            IAccountProductRepository accountProductRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _lookupTypeItemRepository = lookupTypeItemRepository ?? throw new ArgumentNullException(nameof(lookupTypeItemRepository));
            _bookingTypeInclusionRepository = bookingTypeInclusionRepository ?? throw new ArgumentNullException(nameof(bookingTypeInclusionRepository));
            _bookingTypeRepository = bookingTypeRepository ?? throw new ArgumentNullException(nameof(bookingTypeRepository));
            _accountProductRepository = accountProductRepository ?? throw new ArgumentNullException(nameof(accountProductRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetGuestBookingTypes")]
        public IActionResult GetGuestBookingTypes(string fields)
        {

            if (!_typeHelperService.TypeHasProperties<BookingTypeDropdownDto>(fields))
            {
                return BadRequest();
            }

            var guestBookingTypes = _bookingTypeRepository.AllIncludingAsync(o => o.AccountProduct);
            var model = _mapper.Map<IEnumerable<BookingTypeDropdownDto>>(guestBookingTypes);

            return Ok(new BaseRestApiModel
            {
                Payload = model.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpGet("{id}", Name = "GetGuestBookingType")]
        public IActionResult GetGuestBookingType(int id, string fields)
        {

            if (!_typeHelperService.TypeHasProperties<GuestBookingTypeDto>(fields))
            {
                return BadRequest();
            }

            var guestBookingType = _bookingTypeRepository
                .FindAll()
                .Include(o => o.AccountProduct)
                .Include(o => o.BookingTypeInclusions)
                .ThenInclude(o => o.LookupTypeItem)
                .FirstOrDefault(o => o.Id == id);

            var ids = guestBookingType?.BookingTypeInclusions.Select(o => o.InclusionId).ToList();

            var lookupTypeItemRepo = _lookupTypeItemRepository
                .FindBy(o => o.LookupTypeId == _bookingTypeInclusionId && !o.IsDeleted &&
                             !ids.Contains(o.Id))
                .OrderBy(o => o.Description).ToList();

            var model = _mapper.Map<GuestBookingTypeDto>(guestBookingType);

            model.AmenitiesIncluded.AddRange(
                lookupTypeItemRepo.Select(i => new AmenityIncludedDto
                {
                    Id = i.Id,
                    Description = i.Description
                }));

            return Ok(new BaseRestApiModel
            {
                Payload = model.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost("save")]
        public IActionResult Save(GuestAmenityIncludedModel model)
        {
            var repo = _bookingTypeInclusionRepository
                .GetSingleAsync(o => o.BookingTypeId == model.BookingTypeId && o.InclusionId == model.InclusionId)
                .GetAwaiter().GetResult();
            if (repo != null)
            {
                repo.IsActive = model.IsActive;
                repo.UtcDateTimeUpdated = DateTime.Today;
            }
            else
            {
                var entity = new BookingTypeInclusion
                {
                    BookingTypeId = model.BookingTypeId,
                    InclusionId = model.InclusionId,
                    IsActive = model.IsActive,
                    UtcDateTimeCreated = DateTime.Today
                };

                _bookingTypeInclusionRepository.Add(entity);
            }

            _bookingTypeInclusionRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "success",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("save-all")]
        public IActionResult SaveAll(GuestAmenityAllIncludedModel model)
        {

            var lookupTypeItemRepo = _lookupTypeItemRepository.FindBy(o => o.LookupTypeId == _bookingTypeInclusionId && !o.IsDeleted).ToList();
            if (!lookupTypeItemRepo.Any())
            {
                return NotFound();
            }

            lookupTypeItemRepo.ForEach(item =>
            {
                var repo = _bookingTypeInclusionRepository
                    .GetSingleAsync(o => o.BookingTypeId == model.BookingTypeId && o.InclusionId == item.Id)
                    .GetAwaiter().GetResult();
                if (repo != null)
                {
                    repo.IsActive = model.IsAllIncluded;
                    repo.UtcDateTimeUpdated = DateTime.Today;
                }
                else
                {
                    var entity = new BookingTypeInclusion
                    {
                        BookingTypeId = model.BookingTypeId,
                        InclusionId = item.Id,
                        IsActive = model.IsAllIncluded,
                        UtcDateTimeCreated = DateTime.Today
                    };

                    _bookingTypeInclusionRepository.Add(entity);
                }
            });

            _bookingTypeInclusionRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "success",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        #region Guest Booking Types


        [HttpPost("save-booking-type-detail")]
        public IActionResult SaveBookingTypeDetail(GuestBookingTypeModel model)
        {
            var repo = _bookingTypeRepository
                .GetSingleAsync(o => o.Id == model.BookingTypeId)
                .GetAwaiter().GetResult();
            if (repo != null)
            {
                //repo.IsActive = model.IsActive;
                repo.NoOfPersons = model.NoOfPersons;
                repo.NoOfPersonsMax = model.NoOfPersonsMax;
                repo.UtcDateTimeUpdated = DateTime.Today;
                _bookingTypeRepository.Update(repo);
                _bookingTypeRepository.Commit();

                var acctProd = _accountProductRepository
                    .GetSingleAsync(o => o.Id == repo.AccountProductId)
                    .GetAwaiter().GetResult();
                if (acctProd != null)
                {
                    acctProd.OnlinePrice = model.OnlinePrice;
                    acctProd.Price=model.Price;
                    _accountProductRepository.Update(acctProd);
                    _accountProductRepository.Commit();
                }
            }
            else
            {
                //TODO: save new booking type function codes here
            }

           

            return Ok(new BaseRestApiModel
            {
                Payload = "Guest booking type detail has been saved.",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        #endregion
    }
}