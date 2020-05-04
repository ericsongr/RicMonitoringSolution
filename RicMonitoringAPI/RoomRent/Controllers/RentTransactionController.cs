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
using Microsoft.AspNetCore.Authorization;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [Authorize(Policy = "Superuser")]
    [Route("api/rent-transactions")]
    [ApiController]
    public class RentTransactionsController : ControllerBase
    {
        private readonly RoomRentContext _context;
        private readonly IRentTransactionRepository _rentTransactionRepository;
        private readonly IRentTransactionDetailRepository _rentDetailTransactionRepository;
        private readonly IRentTransactionPropertyMappingService _rentTransactionPropertyMappingService;
        private readonly IRenterRepository _renterRepository;
        private readonly IRentArrearRepository _rentArrearRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public RentTransactionsController(RoomRentContext context,
            IRentTransactionRepository rentTransactionRepository,
            IRentTransactionDetailRepository rentDetailTransactionRepository,
            IRentTransactionPropertyMappingService rentTransactionPropertyMappingService,
            IRenterRepository renterRepository, 
            IRentArrearRepository rentArrearRepository,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService)
        {
            _context = context;
            _rentTransactionRepository = rentTransactionRepository;
            _rentDetailTransactionRepository = rentDetailTransactionRepository;
            _rentTransactionPropertyMappingService = rentTransactionPropertyMappingService;
            _renterRepository = renterRepository;
            _rentArrearRepository = rentArrearRepository;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet("{renterId}", Name = "Get")]
        public IActionResult Get(int renterId, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<RentTransaction2Dto>(fields))
            {
                return BadRequest();
            }

            var currentDate = DateTime.Now.Date;
            var rentTransactionFromRepo =  _rentTransactionRepository.GetTransactionQueryResult(currentDate, renterId).SingleOrDefault();
            if (rentTransactionFromRepo == null)
            {
                return NotFound();
            }   

            var rentTransaction = Mapper.Map<RentTransaction2Dto>(rentTransactionFromRepo);

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

            var rentTransactionEntity = Mapper.Map<RentTransaction>(rentTransaction);

            _rentTransactionRepository.Add(rentTransactionEntity);
            _rentTransactionRepository.Commit();

            //save the arrear to transaction detail
            if (rentTransaction.PreviousUnpaidAmount > 0)
            {
                _rentDetailTransactionRepository.Add(new RentTransactionDetail
                {
                    TransactionId = rentTransactionEntity.Id,
                    RentArrearId = rentTransaction.RentArrearId,
                    Amount = rentTransaction.PreviousUnpaidAmount,
                });
            }

            //save transaction detail
            _rentDetailTransactionRepository.Add(new RentTransactionDetail
            {
                TransactionId = rentTransactionEntity.Id,
                Amount = rentTransaction.MonthlyRent,
            });
            _rentDetailTransactionRepository.Commit();

            if (rentTransaction.IsDepositUsed)
            {
                //detect 1 month to field MonthUsed
                AddOrDeductMonthUsed(rentTransactionEntity.RenterId, true);
            }

            ////update previous arrear
            //var updatePreviousArrear = _rentArrearRepository.FindBy(o => o.Id == rentTransaction.RentArrearId).FirstOrDefault();
            //if (updatePreviousArrear != null)
            //{
            //    updatePreviousArrear.IsProcessed = true;
            //    _rentArrearRepository.Update(updatePreviousArrear);
            //}

            //if (rentTransaction.Balance > 0)
            //{
            //    _rentArrearRepository.Add(new RentArrear
            //    {
            //        RenterId = rentTransaction.RenterId,
            //        RentTransactionId = rentTransactionEntity.Id,
            //        UnpaidAmount = rentTransaction.Balance ?? 0,
            //        IsProcessed = false
            //    });
            //}
            //_rentArrearRepository.Commit();

            return CreatedAtRoute("GetAll", new { id = rentTransactionEntity.Id });
        }

        [HttpPut(Name = "BalanceAdjustment")]
        [Route("{transactionId:int}/BalanceAdjustment")]
        public async Task<IActionResult> BalanceAdjustment(int transactionId, [FromBody] RentTransactionBalanceAdjustmentDto balanceAdjustment)
        {
            if (balanceAdjustment == null)
            {
                return NotFound();
            }

            var rentTransactionEntity = await _rentTransactionRepository.GetSingleAsync(balanceAdjustment.TransactionId);
            if (rentTransactionEntity == null)
            {
                return NotFound();
            }

            rentTransactionEntity.AdjustmentBalancePaymentDueAmount = balanceAdjustment.AdjustmentBalancePaymentDueAmount;
            rentTransactionEntity.Note = balanceAdjustment.Note +
                                         $"\n>>Adjustment {balanceAdjustment.AdjustmentBalancePaymentDueAmount} pesos date of {DateTime.Now.ToString("dd-MMM-yyyy")}";

            _rentTransactionRepository.Update(rentTransactionEntity);
            _rentTransactionRepository.Commit();

            var rentArrear = await _rentArrearRepository.GetSingleAsync(o => o.RentTransactionId == balanceAdjustment.TransactionId);
            if (rentArrear != null)
            {
                rentArrear.UnpaidAmount = ((rentTransactionEntity.Balance ?? 0) - balanceAdjustment.AdjustmentBalancePaymentDueAmount);

                _rentArrearRepository.Update(rentArrear);
                _rentArrearRepository.Commit();
            }


            var fields =
                "id,renterName,renterId,roomName,roomId,monthlyRent,dueDate,dueDateString,period,paidDate," +
                "paidAmount,balance,balanceDateToBePaid,previousUnpaidAmount,rentArrearId,totalAmountDue,isDepositUsed," +
                "note,transactionType,isNoAdvanceDepositLeft,isProcessed,adjustmentBalancePaymentDueAmount,isBalanceEditable";

            return Redirect($"/api/rent-transactions/{rentTransactionEntity.RenterId}?fields={fields}");

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

            //if change existing transacton from used deposit to just pay the rent 
            if (rentTransactionEntity.IsDepositUsed != rentTransaction.IsDepositUsed)
            {
                if (rentTransaction.IsDepositUsed)
                    AddOrDeductMonthUsed(rentTransaction.RenterId, true);
                else
                    AddOrDeductMonthUsed(rentTransaction.RenterId, false);

            }

            rentTransactionEntity.PaidDate = rentTransaction.PaidDate;
            rentTransactionEntity.PaidAmount = rentTransaction.PaidAmount;
            rentTransactionEntity.Balance = rentTransaction.Balance;
            rentTransactionEntity.BalanceDateToBePaid = rentTransaction.BalanceDateToBePaid;
            rentTransactionEntity.IsDepositUsed = rentTransaction.IsDepositUsed;
            rentTransactionEntity.Note = rentTransaction.Note;
            
            _rentTransactionRepository.Update(rentTransactionEntity);
            _rentTransactionRepository.Commit();

            return CreatedAtRoute("GetAll", new { id = rentTransactionEntity.Id });

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
            var rentTransactionEntity = await _rentTransactionRepository.GetSingleAsync(id);
            if (rentTransactionEntity == null)
            {
                return NotFound();
            }

            _rentTransactionRepository.Delete(rentTransactionEntity);
            _rentTransactionRepository.Commit();

            return Ok(new { message = "Rent Transaction successfully deleted."});
        }

    }
}
