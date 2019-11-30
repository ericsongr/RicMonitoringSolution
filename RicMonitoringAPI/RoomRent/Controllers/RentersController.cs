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

namespace RicMonitoringAPI.RenterRent.Controllers
{
    [Route("api/renters")]
    [ApiController]
    public class RentersController : ControllerBase
    {
        private readonly RoomRentContext _context;
        private readonly IRenterRepository _renterRepository;
        private readonly IRenterPropertyMappingService _renterPropertyMappingService;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public RentersController(RoomRentContext context,
            IRenterRepository renterRepository,
            IRenterPropertyMappingService renterPropertyMappingService,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService)
        {
            _context = context;
            _renterRepository = renterRepository;
            _renterPropertyMappingService = renterPropertyMappingService;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet("{id}", Name = "GetRenter")]
        public async Task<IActionResult> GetRenter(int id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<RenterDto>(fields))
            {
                return BadRequest();
            }

            var RenterFromRepo = await _renterRepository.GetSingleAsync(o => o.Id == id);
            if (RenterFromRepo == null)
            {
                return NotFound();
            }

            var Renter = Mapper.Map<RenterDto>(RenterFromRepo);

            return Ok(Renter.ShapeData(fields));
        }

        // GET: api/Renters
        [HttpGet(Name = "GetRenters")]
        public IActionResult GetRenters([FromQuery] RenterResourceParameters RenterResourceParameters)
        {
            if (!_renterPropertyMappingService.ValidMappingExistsFor<RenterDto, Renter>
                (RenterResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<RenterDto>
                (RenterResourceParameters.Fields))
            {
                return BadRequest();
            }

            var RenterFromRepo = _renterRepository.GetRenters(RenterResourceParameters);

            var previousPageLink = RenterFromRepo.HasPrevious
                ? CreateRenterResourceUri(RenterResourceParameters,
                    ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = RenterFromRepo.HasPrevious
                ? CreateRenterResourceUri(RenterResourceParameters,
                    ResourceUriType.NextPage)
                : null;

            var paginationMetaData = new
            {
                totalCount = RenterFromRepo.TotalCount,
                pageSize = RenterFromRepo.PageSize,
                currentPage = RenterFromRepo.CurrentPage,
                totalPages = RenterFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

            var Renters = Mapper.Map<IEnumerable<RenterDto>>(RenterFromRepo);

            return Ok(Renters.ShapeData(RenterResourceParameters.Fields));

        }

        [HttpPost()]
        public IActionResult CreateRenter([FromBody] RenterForCreateDto Renter)
        {
            if (Renter == null)
            {
                return NotFound();
            }

            var RenterEntity = Mapper.Map<Renter>(Renter);

            _renterRepository.Add(RenterEntity);
            _renterRepository.Commit();

            var RenterToReturn = Mapper.Map<RenterDto>(RenterEntity);

            return CreatedAtRoute("GetRenters", new { id = RenterToReturn.Id }, RenterToReturn);
        }

        [HttpPut("{id}", Name = "UpdateRenter")]
        public async Task<IActionResult> UpdateRenter(int id, [FromBody] RenterForUpdateDto renter)
        {
            if (renter == null)
            {
                return NotFound();
            }

            var renterEntity = await _renterRepository.GetSingleAsync(id);
            if (renterEntity == null)
            {
                return NotFound();
            }

            renterEntity.Name = renter.Name;
            renterEntity.AdvanceMonths = renter.AdvanceMonths;
            renterEntity.MonthsUsed = renter.MonthsUsed;
            renterEntity.AdvancePaidDate = renter.AdvancePaidDate;
            renterEntity.StartDate = renter.StartDate;
            renterEntity.DueDate = renter.DueDate;
            renterEntity.NoOfPersons = renter.NoOfPersons;
            renterEntity.RoomId = renter.RoomId;

            renterEntity.TotalPaidAmount = renter.TotalPaidAmount;
            renterEntity.BalanceAmount = renter.BalanceAmount;
            renterEntity.BalancePaidDate = renter.BalancePaidDate;

            renterEntity.IsEndRent = renter.IsEndRent;
            renterEntity.DateEndRent = renter.IsEndRent ? renter.DateEndRent : null;

            _renterRepository.Update(renterEntity);
            _renterRepository.Commit();

            var renterToReturn = Mapper.Map<RenterDto>(renterEntity);

            return CreatedAtRoute("GetRenters", new { id = renterToReturn.Id }, renterToReturn);

        }

        private string CreateRenterResourceUri(
                            RenterResourceParameters renterResourceParamaters,
                            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetRenters",
                        new
                        {
                            fields = renterResourceParamaters.Fields,
                            orderBy = renterResourceParamaters.OrderBy,
                            searchQuery = renterResourceParamaters.SearchQuery,
                            pageNumber = renterResourceParamaters.PageNumber - 1,
                            pageSize = renterResourceParamaters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetRenters",
                        new
                        {
                            fields = renterResourceParamaters.Fields,
                            orderBy = renterResourceParamaters.OrderBy,
                            searchQuery = renterResourceParamaters.SearchQuery,
                            pageNumber = renterResourceParamaters.PageNumber + 1,
                            pageSize = renterResourceParamaters.PageSize
                        });

                default:
                    return _urlHelper.Link("GetRenters",
                            new
                            {
                                fields = renterResourceParamaters.Fields,
                                orderBy = renterResourceParamaters.OrderBy,
                                searchQuery = renterResourceParamaters.SearchQuery,
                                pageNumber = renterResourceParamaters.PageNumber,
                                pageSize = renterResourceParamaters.PageSize
                            });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Renter>> DeleteRenter(int id)
        {
            var renterEntity = await _renterRepository.GetSingleAsync(id);
            if (renterEntity == null)
            {
                return NotFound();
            }

            _renterRepository.Delete(renterEntity);
            _renterRepository.Commit();

            return Ok(new { message = "Renter successfully deleted."});
        }

    }
}
