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
using RicEntityFramework.Services;
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
        private readonly IMapper _mapper;


        public GuestRoomAvailabilityController(
            ILookupTypeRepository lookupTypeRepository,
            IGuestBookingDetailRepository guestBookingDetailRepository,
            ITypeHelperService typeHelperService,
            IImageService imageService,
            IMapper mapper)
        {
            _lookupTypeRepository = lookupTypeRepository ?? throw new ArgumentNullException(nameof(lookupTypeRepository));
            _guestBookingDetailRepository = guestBookingDetailRepository ?? throw new ArgumentNullException(nameof(guestBookingDetailRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
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

            var lookupTypeItems = _mapper.Map<IEnumerable<GuestRoomTypesAvailabilityDto>>(lookupTypes).ToList();
            lookupTypeItems.ForEach(item =>
            {
                item.GuestRoomsOrBeds.ForEach(myItem =>
                {
                    var occupied = occupiedRoomOrBed.FirstOrDefault(o => o.RoomOrBedId == myItem.Id);
                    if (occupied != null)
                        myItem.GuestId = occupied.Id;
                    
                    myItem.DefaultImage = _imageService.GetImageInBase64($"default-room.png", "GuestRoom");
                });
            });

            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        //[HttpGet(Name = "GuestRoomImages")]
        //[Route("images/{id}")]
        //public async Task<IActionResult> GuestRoomImages(int id)
        //{
        //    //var base64S = new List<string>();
        //    //var tool = _toolInventoryRepository.GetSingleAsync(o => o.Id == id).GetAwaiter().GetResult();
        //    //if (tool != null)
        //    //{
        //    //    var filenames = tool.Images.Split(',');
        //    //    foreach (var filename in filenames)
        //    //    {
        //    //        base64S.Add(_imageService.GetImageInBase64($"{filename}", "InventoryToolsImage"));
        //    //    }
        //    //}

        //    var base64S = _imageService.GetImageInBase64($"default-room", "InventoryToolsImage");

        //    return Ok(new BaseRestApiModel
        //    {
        //        Payload = base64S,
        //        Errors = new List<BaseError>(),
        //        StatusCode = (int)HttpStatusCode.OK
        //    });

        //}

    }
}