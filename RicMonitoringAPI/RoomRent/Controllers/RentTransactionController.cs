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

namespace RicMonitoringAPI.RentTransactionRent.Controllers
{
    [Route("api/rent-transactions")]
    [ApiController]
    public class RentTransactionsController : ControllerBase
    {
        private readonly RoomRentContext _context;
        private readonly IRentTransactionRepository _rentTransactionRepository;
        private readonly IRentTransactionPropertyMappingService _rentTransactionPropertyMappingService;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public RentTransactionsController(RoomRentContext context,
            IRentTransactionRepository rentTransactionRepository,
            IRentTransactionPropertyMappingService rentTransactionPropertyMappingService,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService)
        {
            _context = context;
            _rentTransactionRepository = rentTransactionRepository;
            _rentTransactionPropertyMappingService = rentTransactionPropertyMappingService;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<RentTransactionDto>(fields))
            {
                return BadRequest();
            }

            var rentTransactionFromRepo = await _rentTransactionRepository.GetSingleAsync(o => o.Id == id);
            if (rentTransactionFromRepo == null)
            {
                return NotFound();
            }

            var rentTransaction = Mapper.Map<RentTransactionDto>(rentTransactionFromRepo);

            return Ok(rentTransaction.ShapeData(fields));
        }

        // GET: api/RentTransactions
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll([FromQuery] RentTransactionResourceParameters rentTransactionResourceParameters)
        {
            if (!_rentTransactionPropertyMappingService.ValidMappingExistsFor<RentTransaction2Dto, RentTransaction2>
                (rentTransactionResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<RentTransaction2Dto>
                (rentTransactionResourceParameters.Fields))
            {
                return BadRequest();
            }

            var rentTransactionFromRepo = _rentTransactionRepository.GetRentTransactions(rentTransactionResourceParameters);
             
            var previousPageLink = rentTransactionFromRepo.HasPrevious
                ? CreateResourceUri(rentTransactionResourceParameters,
                    ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = rentTransactionFromRepo.HasPrevious
                ? CreateResourceUri(rentTransactionResourceParameters,
                    ResourceUriType.NextPage)
                : null;

            var paginationMetaData = new
            {
                totalCount = rentTransactionFromRepo.TotalCount,
                pageSize = rentTransactionFromRepo.PageSize,
                currentPage = rentTransactionFromRepo.CurrentPage,
                totalPages = rentTransactionFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

            var rentTransactions = Mapper.Map<IEnumerable<RentTransaction2Dto>>(rentTransactionFromRepo);

            return Ok(rentTransactions.ShapeData(rentTransactionResourceParameters.Fields));

        }

        [HttpPost()]
        public IActionResult Create([FromBody] RentTransactionForCreateDto rentTransaction)
        {
            if (rentTransaction == null)
            {
                return NotFound();
            }

            var RentTransactionEntity = Mapper.Map<RentTransaction>(rentTransaction);

            _rentTransactionRepository.Add(RentTransactionEntity);
            _rentTransactionRepository.Commit();

            var RentTransactionToReturn = Mapper.Map<RentTransactionDto>(RentTransactionEntity);

            return CreatedAtRoute("GetAll", new { id = RentTransactionToReturn.Id }, RentTransactionToReturn);
        }

        [HttpPut("{id}", Name = "Update")]
        public async Task<IActionResult> Update(int id, [FromBody] RentTransactionForUpdateDto rentTransaction)
        {
            if (rentTransaction == null)
            {
                return NotFound();
            }

            var rentTransactionEntity = await _rentTransactionRepository.GetSingleAsync(id);
            if (rentTransactionEntity == null)
            {
                return NotFound();
            }

            rentTransactionEntity.PaidDate = rentTransaction.PaidDate;
            rentTransactionEntity.Amount = rentTransaction.Amount;
            rentTransactionEntity.Balance = rentTransaction.Balance;
            rentTransactionEntity.BalanceDateToBePaid = rentTransaction.BalanceDateToBePaid;
            rentTransactionEntity.IsDepositUsed = rentTransaction.IsDepositUsed;
            rentTransactionEntity.Note = rentTransaction.Note;
            rentTransactionEntity.DueDate = rentTransaction.DueDate;

            _rentTransactionRepository.Update(rentTransactionEntity);
            _rentTransactionRepository.Commit();

            var RentTransactionToReturn = Mapper.Map<RentTransactionDto>(rentTransactionEntity);

            return CreatedAtRoute("GetAll", new { id = RentTransactionToReturn.Id }, RentTransactionToReturn);

        }

        private string CreateResourceUri(
                            RentTransactionResourceParameters rentTransactionResourceParamaters,
                            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAll",
                        new
                        {
                            fields = rentTransactionResourceParamaters.Fields,
                            orderBy = rentTransactionResourceParamaters.OrderBy,
                            searchQuery = rentTransactionResourceParamaters.SearchQuery,
                            pageNumber = rentTransactionResourceParamaters.PageNumber - 1,
                            pageSize = rentTransactionResourceParamaters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAll",
                        new
                        {
                            fields = rentTransactionResourceParamaters.Fields,
                            orderBy = rentTransactionResourceParamaters.OrderBy,
                            searchQuery = rentTransactionResourceParamaters.SearchQuery,
                            pageNumber = rentTransactionResourceParamaters.PageNumber + 1,
                            pageSize = rentTransactionResourceParamaters.PageSize
                        });

                default:
                    return _urlHelper.Link("GetAll",
                            new
                            {
                                fields = rentTransactionResourceParamaters.Fields,
                                orderBy = rentTransactionResourceParamaters.OrderBy,
                                searchQuery = rentTransactionResourceParamaters.SearchQuery,
                                pageNumber = rentTransactionResourceParamaters.PageNumber,
                                pageSize = rentTransactionResourceParamaters.PageSize
                            });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RentTransaction>> DeleteRentTransaction(int id)
        {
            var RentTransactionEntity = await _rentTransactionRepository.GetSingleAsync(id);
            if (RentTransactionEntity == null)
            {
                return NotFound();
            }

            _rentTransactionRepository.Delete(RentTransactionEntity);
            _rentTransactionRepository.Commit();

            return Ok(new { message = "Rent Transaction successfully deleted."});
        }

    }
}
