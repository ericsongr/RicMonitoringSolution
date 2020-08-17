using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.Parameters;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Enumerations;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    //[AllowAnonymous]
    [Authorize(Policy = "SuperAndAdmin")]
    [Route("api/rent-transactions")]
    [ApiController]
    public class RentTransactionsController : ControllerBase
    {
        private readonly IRentTransactionRepository _rentTransactionRepository;
        private readonly IRentTransactionDetailRepository _rentDetailTransactionRepository;
        private readonly IRentTransactionPropertyMappingService _rentTransactionPropertyMappingService;
        private readonly IRenterRepository _renterRepository;
        private readonly IRentArrearRepository _rentArrearRepository;
        private readonly IRentTransactionPaymentRepository _rentTransactionPaymentRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public RentTransactionsController(
            IRentTransactionRepository rentTransactionRepository,
            IRentTransactionDetailRepository rentDetailTransactionRepository,
            IRentTransactionPropertyMappingService rentTransactionPropertyMappingService,
            IRenterRepository renterRepository,
            IRentArrearRepository rentArrearRepository,
            IRentTransactionPaymentRepository rentTransactionPaymentRepository,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService)
        {
            _rentTransactionRepository = rentTransactionRepository ?? throw new ArgumentNullException(nameof(rentTransactionRepository));
            _rentDetailTransactionRepository = rentDetailTransactionRepository ?? throw new ArgumentNullException(nameof(rentDetailTransactionRepository));
            _rentTransactionPropertyMappingService = rentTransactionPropertyMappingService ?? throw new ArgumentNullException(nameof(rentTransactionPropertyMappingService));
            _renterRepository = renterRepository ?? throw new ArgumentNullException(nameof(renterRepository));
            _rentArrearRepository = rentArrearRepository ?? throw new ArgumentNullException(nameof(rentArrearRepository));
            _rentTransactionPaymentRepository = rentTransactionPaymentRepository ?? throw new ArgumentNullException(nameof(rentTransactionPaymentRepository));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<RentTransaction2Dto>(fields))
            {
                return BadRequest();
            }

            var rentTransactionFromRepo = _rentTransactionRepository.GetTransaction(id).First();
            if (rentTransactionFromRepo == null)
            {
                return NotFound();
            }

            var rentTransaction = Mapper.Map<RentTransaction2Dto>(rentTransactionFromRepo);
            rentTransaction.Payments =
                Mapper.Map<IEnumerable<RentTransactionPaymentDto>>(rentTransactionFromRepo.RentTransactionPayments)
                    .OrderByDescending(o => o.DatePaid)
                    .ToList();

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
        
        [HttpPut("{id}", Name = "Update")]
        public async Task<IActionResult> Update(int id, [FromBody] RentTransactionForUpdateDto rentTransaction)
        {
            if (rentTransaction == null)
            {
                return NotFound();
            }

            if (DateTime.TryParse(rentTransaction.PaidDateInput, out DateTime paidDate))
                rentTransaction.PaidDate = paidDate;

            if (DateTime.TryParse(rentTransaction.BalanceDateToBePaidInput, out DateTime balanceDateToBePaid))
                rentTransaction.BalanceDateToBePaid = balanceDateToBePaid;

            var rentTransactionEntity = await _rentTransactionRepository
                .GetSingleIncludesAsync(o => o.Id == id,
                            o => o.RentTransactionPayments);
            if (rentTransactionEntity == null)
            {
                return NotFound();
            }

            if (rentTransaction.IsAddingPayment)
            {
                //save payment history
                _rentTransactionPaymentRepository.Add(new RentTransactionPayment
                {
                    Amount = rentTransaction.PaidAmount ?? 0,
                    DatePaid = rentTransaction.PaidDate.Value,
                    PaymentTransactionType = PaymentTransactionType.Paid,
                    RentTransactionId = id
                });
            }
            else
            {
                if (rentTransaction.IsDepositUsed && !rentTransaction.IsEditingPayment)
                {
                    //when use deposit date paid should be current date of entry
                    rentTransaction.PaidDate = DateTime.Now.Date;

                    AddOrDeductMonthUsed(rentTransaction.RenterId, true);

                    _rentTransactionPaymentRepository.Add(new RentTransactionPayment
                    {
                        Amount = 0,
                        DatePaid = rentTransaction.PaidDate.Value.Date,
                        PaymentTransactionType = PaymentTransactionType.DepositUsed,
                        RentTransactionId = id
                    });
                }
                else
                {

                    if (rentTransaction.IsEditingPayment)
                    {
                        var payment = _rentTransactionPaymentRepository
                            .GetSingleAsync(o => o.Id == rentTransaction.RentTransactionPaymentId)
                            .GetAwaiter()
                            .GetResult();
                        if (payment != null)
                        {
                            payment.Amount = rentTransaction.PaidAmount ?? 0;
                            payment.DatePaid = rentTransaction.PaidDate.Value;
                            _rentTransactionPaymentRepository.Update(payment);
                            _rentTransactionPaymentRepository.Commit();
                        }
                    }
                    else
                    {
                        //save payment history
                        _rentTransactionPaymentRepository.Add(new RentTransactionPayment
                        {
                            Amount = rentTransaction.PaidAmount ?? 0,
                            DatePaid = rentTransaction.PaidDate.Value,
                            PaymentTransactionType = PaymentTransactionType.Paid,
                            RentTransactionId = id
                        });
                    }
                }
            }

            //commit
            _rentTransactionPaymentRepository.Commit();

            //get total
            decimal totalPaidAmount = GetTotalPaidAmount(id);
            //get excess payment if had.
            decimal excessPaidAmount = 0;
            if (rentTransaction.Balance == null || rentTransaction.Balance == 0)
            {
                if (rentTransactionEntity.RentTransactionPayments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed))
                {
                    //don't deduct monthly due if ticked deposit within the month
                    excessPaidAmount = totalPaidAmount;
                }
                else if (totalPaidAmount > rentTransaction.TotalAmountDue)
                {
                    excessPaidAmount = totalPaidAmount - rentTransaction.TotalAmountDue;
                }
            }

            rentTransactionEntity.PaidDate = rentTransaction.PaidDate;
            rentTransactionEntity.PaidAmount = totalPaidAmount;
            rentTransactionEntity.ExcessPaidAmount = excessPaidAmount;
            rentTransactionEntity.Balance = rentTransaction.Balance;
            rentTransactionEntity.BalanceDateToBePaid = rentTransaction.BalanceDateToBePaid;
            //rentTransactionEntity.IsDepositUsed = rentTransaction.IsDepositUsed;
            rentTransactionEntity.Note = rentTransaction.Note;

            _rentTransactionRepository.Update(rentTransactionEntity);
            _rentTransactionRepository.Commit();

            return CreatedAtRoute("GetAll", new { id = rentTransactionEntity.Id });

        }

        /// <summary>
        /// total paid amount of the renter filter by transaction id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private decimal GetTotalPaidAmount(int id)
        {
            return _rentTransactionPaymentRepository
                .FindBy(o => o.RentTransactionId == id)
                .Sum(o => o.Amount);
        }

        private void AddOrDeductMonthUsed(int renterId, bool isAdd)
        {
            var renter = _renterRepository.GetSingleAsync(o => o.Id == renterId);
            if (renter != null)
            {
                renter.Result.MonthsUsed =
                    (isAdd ?
                        renter.Result.MonthsUsed + 1 :
                        renter.Result.MonthsUsed - 1);

                _renterRepository.Update(renter.Result);
                _renterRepository.Commit();
            }
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
            var rentTransactionEntity = await _rentTransactionRepository.GetSingleAsync(o => o.Id == id);
            if (rentTransactionEntity == null)
            {
                return NotFound();
            }

            _rentTransactionRepository.Delete(rentTransactionEntity);
            _rentTransactionRepository.Commit();

            return Ok(new { message = "Rent Transaction successfully deleted." });
        }

    }
}
