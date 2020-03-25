using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.Api.Helpers;
using AutoMapper;
using RicMonitoringAPI.Common.Enumeration;

namespace RicMonitoringAPI.RenterRent.Controllers
{
    [Route("api/renters")]
    [ApiController]
    public class RentersController : ControllerBase
    {
        private readonly RoomRentContext _context;
        private readonly IRenterRepository _renterRepository;
        private readonly IRentTransactionRepository _rentTransactionRepository;
        private readonly IRenterPropertyMappingService _renterPropertyMappingService;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public RentersController(RoomRentContext context,
            IRenterRepository renterRepository,
            IRentTransactionRepository rentTransactionRepository,
            IRenterPropertyMappingService renterPropertyMappingService,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService)
        {
            _context = context;
            _renterRepository = renterRepository;
            _rentTransactionRepository = rentTransactionRepository;
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
        public IActionResult GetRenters([FromQuery] RenterResourceParameters renterResourceParameters)
        {
            if (!_renterPropertyMappingService.ValidMappingExistsFor<RenterDto, Renter>
                (renterResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<RenterDto>
                (renterResourceParameters.Fields))
            {
                return BadRequest();
            }

            var RenterFromRepo = _renterRepository.GetRenters(renterResourceParameters);

            var previousPageLink = RenterFromRepo.HasPrevious
                ? CreateRenterResourceUri(renterResourceParameters,
                    ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = RenterFromRepo.HasPrevious
                ? CreateRenterResourceUri(renterResourceParameters,
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

            return Ok(Renters.ShapeData(renterResourceParameters.Fields));

        }

        [HttpPost()]
        public IActionResult CreateRenter([FromBody] RenterForCreateDto renter)
        {
            if (renter == null)
            {
                return NotFound();
            }

            var renterEntity = Mapper.Map<Renter>(renter);

            _renterRepository.Add(renterEntity);
            _renterRepository.Commit();

            //add data to rent transaction table
            var dueDate = new DateTime(renter.StartDate.Year, renter.StartDate.Month, renter.DueDay);
            var startDate = dueDate.AddDays(1);
            var endDate = dueDate.AddMonths(1);

            var rentTransaction = new RentTransaction
            {
                PaidDate = renter.AdvancePaidDate,
                PaidAmount = renter.TotalPaidAmount,
                Balance = renter.BalanceAmount,
                BalanceDateToBePaid = renter.BalancePaidDate,
                Note = "Advance/Deposit",
                RoomId = renter.RoomId,
                RenterId = renterEntity.Id,
                DueDate = dueDate,
                IsDepositUsed = true,
                Period = $"{startDate.ToString("dd-MMM")} to {endDate.ToString("dd-MMM-yyyy")}",
                TransactionType = TransactionTypeEnum.AdvanceAndDeposit
            };
            _rentTransactionRepository.Add(rentTransaction);
            _rentTransactionRepository.Commit();

            //update renter table with the generated rent transaction id
            renterEntity.MonthsUsed = (renterEntity.MonthsUsed + 1); // tagged 1 meaning advance will use to ongoing month period
            renterEntity.RentTransactionId = rentTransaction.Id;
            _renterRepository.Update(renterEntity);
            _renterRepository.Commit();

            var renterToReturn = Mapper.Map<RenterDto>(renterEntity);

            return CreatedAtRoute("GetRenters", new { id = renterToReturn.Id });
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
            renterEntity.DueDay = renter.DueDay;
            renterEntity.NoOfPersons = renter.NoOfPersons;
            renterEntity.RoomId = renter.RoomId;

            renterEntity.TotalPaidAmount = renter.TotalPaidAmount;
            renterEntity.BalanceAmount = renter.BalanceAmount;
            renterEntity.BalancePaidDate = renter.BalancePaidDate;

            renterEntity.IsEndRent = renter.IsEndRent;
            renterEntity.DateEndRent = renter.IsEndRent ? renter.DateEndRent : null;

            _renterRepository.Update(renterEntity);
            _renterRepository.Commit();

            //fetch data from rent transaction table
            var rentTransaction = _rentTransactionRepository.FindBy(o => o.Id == renterEntity.RentTransactionId).FirstOrDefault();
            if (rentTransaction != null)
            {
                //update data to rent transaction table
                var dueDate = new DateTime(renter.StartDate.Year, renter.StartDate.Month, renter.DueDay);
                var startDate = dueDate.AddDays(1);
                var endDate = dueDate.AddMonths(1);

                rentTransaction.PaidDate = renter.AdvancePaidDate;
                rentTransaction.PaidAmount = renter.TotalPaidAmount;
                rentTransaction.Balance = renter.BalanceAmount;
                rentTransaction.BalanceDateToBePaid = renter.BalancePaidDate;
                rentTransaction.Note = $"Advance/Deposit - Edit";
                rentTransaction.RoomId = renter.RoomId;
                rentTransaction.RenterId = renter.Id;
                rentTransaction.DueDate = dueDate;
                rentTransaction.Period = $"{startDate.ToString("dd-MMM")} to {endDate.ToString("dd-MMM-yyyy")}";
                _rentTransactionRepository.Update(rentTransaction);
                _rentTransactionRepository.Commit();
            }

            var renterToReturn = Mapper.Map<RenterDto>(renterEntity);

            return CreatedAtRoute("GetRenters", new { id = renterToReturn.Id});

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
