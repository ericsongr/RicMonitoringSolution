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
using RicModel.Enumeration;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Enumerations;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    //[AllowAnonymous]
    [Authorize(Policy = "SuperAndAdmin")]
    [Route("api/renters")]
    [ApiController]
    public class RentersController : ControllerBase
    {
        private readonly IRenterRepository _renterRepository;
        private readonly IRentTransactionRepository _rentTransactionRepository;
        private readonly IRentTransactionPaymentRepository _rentTransactionPaymentRepository;
        private readonly IRenterPropertyMappingService _renterPropertyMappingService;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public RentersController(
            IRenterRepository renterRepository,
            IRentTransactionRepository rentTransactionRepository,
            IRentTransactionPaymentRepository rentTransactionPaymentRepository,
            IRenterPropertyMappingService renterPropertyMappingService,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService)
        {
            _renterRepository = renterRepository ?? throw new ArgumentNullException(nameof(renterRepository));
            _rentTransactionRepository = rentTransactionRepository ?? throw new ArgumentNullException(nameof(rentTransactionRepository));
            _rentTransactionPaymentRepository = rentTransactionPaymentRepository ?? throw new ArgumentNullException(nameof(rentTransactionPaymentRepository));
            _renterPropertyMappingService = renterPropertyMappingService ?? throw new ArgumentNullException(nameof(renterPropertyMappingService));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
        }

        [HttpGet("{id}", Name = "GetRenter")]
        public async Task<IActionResult> GetRenter(int id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<RenterDto>(fields))
            {
                return BadRequest();
            }

            var renterFromRepo = await _renterRepository.GetSingleAsync(o => o.Id == id);
            if (renterFromRepo == null)
            {
                return NotFound();
            }

            var Renter = Mapper.Map<RenterDto>(renterFromRepo);

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

            var renterFromRepo = _renterRepository.GetRenters(renterResourceParameters);

            var previousPageLink = renterFromRepo.HasPrevious
                ? CreateRenterResourceUri(renterResourceParameters,
                    ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = renterFromRepo.HasPrevious
                ? CreateRenterResourceUri(renterResourceParameters,
                    ResourceUriType.NextPage)
                : null;

            var paginationMetaData = new
            {
                totalCount = renterFromRepo.TotalCount,
                pageSize = renterFromRepo.PageSize,
                currentPage = renterFromRepo.CurrentPage,
                totalPages = renterFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

            var Renters = Mapper.Map<IEnumerable<RenterDto>>(renterFromRepo);

            return Ok(Renters.ShapeData(renterResourceParameters.Fields));

        }

        [HttpPost()]
        public IActionResult CreateRenter([FromBody] RenterForCreateDto renter)
        {
            if (renter == null)
            {
                return NotFound();
            }

            DateTime.TryParse(renter.StartDateInput, out DateTime startDateInput);
            DateTime.TryParse(renter.AdvancePaidDateInput, out DateTime advancePaidDateInput);

            renter.StartDate = startDateInput;
            renter.AdvancePaidDate = advancePaidDateInput;
            renter.PreviousDueDate = startDateInput;
            renter.NextDueDate = startDateInput.AddMonths(1);

            var renterEntity = Mapper.Map<Renter>(renter);

            _renterRepository.Add(renterEntity);
            _renterRepository.Commit();

            //add data to rent transaction table
            var dueDate = new DateTime(renter.StartDate.Year, renter.StartDate.Month, renter.DueDay);
            var startDate = dueDate.AddDays(1);
            var endDate = dueDate.AddMonths(1);

            var rentTransaction = new RentTransaction
            {
                PaidDate = advancePaidDateInput,
                PaidAmount = renter.TotalPaidAmount,
                TotalAmountDue = renter.TotalPaidAmount,
                Balance = renter.BalanceAmount,
                BalanceDateToBePaid = renter.BalancePaidDate,
                Note = "Advance/Deposit",
                RoomId = renter.RoomId,
                RenterId = renterEntity.Id,
                DueDate = dueDate,
                Period = $"{startDate.ToString("dd-MMM")} to {endDate.ToString("dd-MMM-yyyy")}",
                TransactionType = TransactionTypeEnum.AdvanceAndDeposit
            };
            _rentTransactionRepository.Add(rentTransaction);
            _rentTransactionRepository.Commit();

            ProcessRentTransactionPayment(renter, rentTransaction.Id);

            var renterToReturn = Mapper.Map<RenterDto>(renterEntity);

            return CreatedAtRoute("GetRenters", new { id = renterToReturn.Id, name = renterToReturn.Name.ToLower().Replace(" ", "-") });
        }

        [HttpPut("{id}", Name = "UpdateRenter")]
        public async Task<IActionResult> UpdateRenter(int id, [FromBody] RenterForUpdateDto renter)
        {
            if (renter == null)
            {
                return NotFound();
            }

            var renterEntity = await _renterRepository.GetSingleAsync(o => o.Id == id);
            if (renterEntity == null)
            {
                return NotFound();
            }

            DateTime.TryParse(renter.StartDateInput,out DateTime startDateInput);
            DateTime.TryParse(renter.AdvancePaidDateInput,out DateTime advancePaidDateInput);

            renterEntity.Name = renter.Name;
            renterEntity.AdvanceMonths = renter.AdvanceMonths;
            renterEntity.MonthsUsed = renter.MonthsUsed;
            renterEntity.AdvancePaidDate = advancePaidDateInput;
            renterEntity.StartDate = startDateInput;
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
            var rentTransaction = _rentTransactionRepository.FindBy(o => o.RenterId == renterEntity.Id && o.TransactionType == TransactionTypeEnum.AdvanceAndDeposit).FirstOrDefault();
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

                ProcessRentTransactionPayment(renter, rentTransaction.Id);
            }

            var renterToReturn = Mapper.Map<RenterDto>(renterEntity);

            return CreatedAtRoute("GetRenters", new { id = renterToReturn.Id, name = renterToReturn.Name.ToLower().Replace(" ", "-") });

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Renter>> DeleteRenter(int id)
        {
            var renterEntity = await _renterRepository.GetSingleAsync(o => o.Id == id);
            if (renterEntity == null)
            {
                return NotFound();
            }

            _renterRepository.Delete(renterEntity);
            _renterRepository.Commit();

            return Ok(new { message = "Renter successfully deleted."});
        }

        #region Private Methods

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

        /// <summary>
        /// if new renter insert to RentTransactionPayments table
        /// if update renter mark as deleted the existing one and insert new payment detail.
        /// </summary>
        /// <param name="renter"></param>
        /// <param name="rentTransactionId"></param>
        private void ProcessRentTransactionPayment(Renter renter, int rentTransactionId)
        {
            var payment =
                _rentTransactionPaymentRepository.GetSingleAsync(o => o.RentTransactionId == rentTransactionId)
                    .GetAwaiter().GetResult();
            if (payment == null)
            {
                //insert to RentTransactionPayments table
                _rentTransactionPaymentRepository.Add(new RentTransactionPayment
                {
                    PaymentTransactionType = PaymentTransactionType.AdvanceAndDeposit,
                    Amount = renter.TotalPaidAmount,
                    DatePaid = renter.AdvancePaidDate,
                    RentTransactionId = rentTransactionId
                });
            }
            else
            {
                //insert to RentTransactionPayments table
                payment.Amount = renter.TotalPaidAmount;
                payment.DatePaid = renter.AdvancePaidDate;
                _rentTransactionPaymentRepository.Update(payment);
            }
            
            _rentTransactionPaymentRepository.Commit();
        }
        
        #endregion

    }
}
