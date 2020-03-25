using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.Api.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using RicMonitoringAPI.RoomRent.Entities.Validators;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomRentContext _context;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomPropertyMappingService _roomPropertyMappingService;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public RoomsController(RoomRentContext context,
            IRoomRepository roomRepository,
            IRoomPropertyMappingService roomPropertyMappingService,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService)
        {
            _context = context;
            _roomRepository = roomRepository;
            _roomPropertyMappingService = roomPropertyMappingService;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet("{id}", Name = "GetRoom")]
        public async Task<IActionResult> GetRoom(int id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<RoomDto>(fields))
            {
                return BadRequest();
            }

            var roomFromRepo = await _roomRepository.GetSingleAsync(o => o.Id == id);
            if (roomFromRepo == null)
            {
                return NotFound();
            }

            var room = Mapper.Map<RoomDto>(roomFromRepo);

            return Ok(room.ShapeData(fields));
        }

        // GET: api/Rooms
        [HttpGet(Name = "GetRooms")]
        public IActionResult GetRooms([FromQuery] RoomResourceParameters roomResourceParameters)
        {
            if (!_roomPropertyMappingService.ValidMappingExistsFor<RoomDto, Room>
                (roomResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<RoomDto>
                (roomResourceParameters.Fields))
            {
                return BadRequest();
            }

            var roomFromRepo = _roomRepository.GetRooms(roomResourceParameters);


            var previousPageLink = roomFromRepo.HasPrevious
                ? CreateRoomResourceUri(roomResourceParameters,
                    ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = roomFromRepo.HasPrevious
                ? CreateRoomResourceUri(roomResourceParameters,
                    ResourceUriType.NextPage)
                : null;

            var paginationMetaData = new
            {
                totalCount = roomFromRepo.TotalCount,
                pageSize = roomFromRepo.PageSize,
                currentPage = roomFromRepo.CurrentPage,
                totalPages = roomFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

            var rooms = Mapper.Map<IEnumerable<RoomDto>>(roomFromRepo);

            var result = rooms.ShapeData(roomResourceParameters.Fields);

            return Ok(result);

        }

        [HttpPost()]
        public IActionResult CreateRoom([FromBody] RoomForCreateDto room)
        {
            if (room == null)
            {
                return NotFound();
            }

            var roomEntity = Mapper.Map<Room>(room);

            _roomRepository.Add(roomEntity);
            _roomRepository.Commit();

            var roomToReturn = Mapper.Map<RoomDto>(roomEntity);

            return CreatedAtRoute("GetRooms", new { id = roomToReturn.Id }, roomToReturn);
        }

        [HttpPut("{id}", Name = "UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomForUpdateDto room)
        {
            if (room == null)
            {
                return NotFound();
            }

            var roomEntity = await _roomRepository.GetSingleAsync(id);
            if (roomEntity == null)
            {
                return NotFound();
            }

            roomEntity.Name = room.Name;
            roomEntity.Frequency = room.Frequency;
            roomEntity.Price = room.Price;
            _roomRepository.Update(roomEntity);
            _roomRepository.Commit();

            var roomToReturn = Mapper.Map<RoomDto>(roomEntity);

            return CreatedAtRoute("GetRooms", new { id = roomToReturn.Id }, roomToReturn);

        }

        private string CreateRoomResourceUri(
                            RoomResourceParameters roomResourceParamaters,
                            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetRooms",
                        new
                        {
                            fields = roomResourceParamaters.Fields,
                            orderBy = roomResourceParamaters.OrderBy,
                            searchQuery = roomResourceParamaters.SearchQuery,
                            pageNumber = roomResourceParamaters.PageNumber - 1,
                            pageSize = roomResourceParamaters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetRooms",
                        new
                        {
                            fields = roomResourceParamaters.Fields,
                            orderBy = roomResourceParamaters.OrderBy,
                            searchQuery = roomResourceParamaters.SearchQuery,
                            pageNumber = roomResourceParamaters.PageNumber + 1,
                            pageSize = roomResourceParamaters.PageSize
                        });

                default:
                    return _urlHelper.Link("GetRooms",
                            new
                            {
                                fields = roomResourceParamaters.Fields,
                                orderBy = roomResourceParamaters.OrderBy,
                                searchQuery = roomResourceParamaters.SearchQuery,
                                pageNumber = roomResourceParamaters.PageNumber,
                                pageSize = roomResourceParamaters.PageSize
                            });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Room>> DeleteRoom(int id)
        {
            var roomEntity = await _roomRepository.GetSingleAsync(id);
            if (roomEntity == null)
            {
                return NotFound();
            }

            _roomRepository.Delete(roomEntity);
            _roomRepository.Commit();

            return Ok(new { message = "Room successfully deleted."});
        }

    }
}
