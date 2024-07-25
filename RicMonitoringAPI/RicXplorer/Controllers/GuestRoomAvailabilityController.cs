using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicEntityFramework.RicXplorer.Repositories;
using RicEntityFramework.Services;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/guest-room-availability")]
    [ApiController]
    public class GuestRoomAvailabilityController : ControllerBase
    {
        private readonly ILookupTypeRepository _lookupTypeRepository;
        private readonly IGuestBookingDetailRepository _guestBookingDetailRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IImageService _imageService;
        private readonly IBookingTypeRepository _bookingTypeRepository;
        private readonly IMapper _mapper;


        public GuestRoomAvailabilityController(
            ILookupTypeRepository lookupTypeRepository,
            IGuestBookingDetailRepository guestBookingDetailRepository,
            ITypeHelperService typeHelperService,
            IImageService imageService,
            IBookingTypeRepository bookingTypeRepository,
            IMapper mapper)
        {
            _lookupTypeRepository = lookupTypeRepository ?? throw new ArgumentNullException(nameof(lookupTypeRepository));
            _guestBookingDetailRepository = guestBookingDetailRepository ?? throw new ArgumentNullException(nameof(guestBookingDetailRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _bookingTypeRepository = bookingTypeRepository ?? throw new ArgumentNullException(nameof(imageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetGuestRooms")]
        public IActionResult GetGuestRooms(string lookUps, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<LookupTypeDto>(fields))
            {
                return BadRequest();
            }

            var occupiedRoomOrBed = _guestBookingDetailRepository.FindBy(o => o.RoomOrBedId != null && o.CheckedOutDateTime == null);
            var lookUpTypeIds = lookUps.Split(',').Select(int.Parse).ToList();
            var lookupTypes = _lookupTypeRepository.FindBy(o => lookUpTypeIds.Contains(o.Id), o => o.LookupTypeItems, o => o.LookupTypeItems);

            if (lookupTypes == null)
            {
                return NotFound();
            }

            var bookingTypes = _bookingTypeRepository.FindAll(o => o.AccountProduct);

            var lookupTypeItems = _mapper.Map<IEnumerable<GuestRoomTypesAvailabilityDto>>(lookupTypes).ToList();
            lookupTypeItems.ForEach(item =>
            {
                item.GuestRoomsOrBeds.ForEach(myItem =>
                {
                    var occupied = occupiedRoomOrBed.FirstOrDefault(o => o.RoomOrBedId == myItem.Id);
                    if (occupied != null)
                        myItem.GuestId = occupied.Id;

                    var bookingType = bookingTypes.FirstOrDefault(o => o.LinkRooms.Contains(item.Id.ToString()));
                    if (bookingType != null)
                        myItem.BookingType = bookingType.AccountProduct.Name;

                    //myItem.DefaultImage = _imageService.GetImageInBase64($"default-room.png", "GuestRoom");
                });
            });

            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpGet("image/{id}", Name = "GuestRoomImage")]
        public async Task<IActionResult> GuestRoomImage(int id)
        {

            string imageBase64 = string.Empty;
            await Task.Run(() =>
            {
                imageBase64 = _imageService.GetImageInBase64($"default-room.png", "GuestRoom");
            });

            return Ok(new BaseRestApiModel
            {
                Payload = imageBase64,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

    }
}