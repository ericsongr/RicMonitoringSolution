using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.CostMonitoring.Interfaces;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicModel.CostMonitoring;
using RicModel.CostMonitoring.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.CostMonitoring.Controllers
{
    [Route("api/cost-monitoring-transaction")]
    [ApiController]
    public class TransactionCostController : ControllerBase
    {
        private readonly ICostItemRepository _costItemRepository;
        private readonly ITransactionCostRepository _transactionCostRepository;
        private readonly ITypeHelperService _typeHelperService;


        public TransactionCostController(
            ICostItemRepository costItemRepository,
            ITransactionCostRepository transactionCostRepository,
            ITypeHelperService typeHelperService)
        {
            _costItemRepository = costItemRepository ?? throw new ArgumentNullException(nameof(costItemRepository));
            _transactionCostRepository = transactionCostRepository ?? throw new ArgumentNullException(nameof(transactionCostRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
        }

        [HttpGet(Name = "TransactionCosts")]
        public async Task<IActionResult> TransactionCosts(bool isFilterByCurrentMonth, string startDate, string endDate, int costCategoryId, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<TransactionCostDto>(fields))
            {
                return BadRequest();
            }

            var transactionCosts = _transactionCostRepository
                .FindAll(o => o.CostItem, o => o.CostCategory);

            if (isFilterByCurrentMonth)
            {
                transactionCosts = transactionCosts.Where(o => o.TransactionDate.Month == DateTime.Now.Month);

            }
            else
            {
                var isStartDateValid = DateTime.TryParse(startDate, out DateTime startDateOut);
                var isEndDateValid = DateTime.TryParse(endDate, out DateTime endDateOut);
                if (!isStartDateValid)
                    return BadRequest("Invalid start date");
                if (!isEndDateValid)
                    return BadRequest("Invalid end date");
                
                transactionCosts = transactionCosts.Where(o => o.TransactionDate.Date >= startDateOut && o.TransactionDate.Date <= endDateOut);

                if (costCategoryId > 0)
                    transactionCosts = transactionCosts.Where(o => o.CostCategoryId == costCategoryId);
            }

            if (transactionCosts == null)
            {
                return NotFound();
            }

            var data = Mapper.Map<IEnumerable<TransactionCostDto>>(transactionCosts.OrderByDescending(o => o.TransactionDate));

            return Ok(new BaseRestApiModel
            {
                Payload = data.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost(Name = "AddTransactionCost")]
        public IActionResult AddTransactionCost(TransactionCostDto model)
        {
            DateTime.TryParse(model.TransactionDate, out DateTime transDate);
            var entity = new TransactionCost
            {
                TransactionDate = transDate,
                CostItemId = model.CostItemId,
                CostCategoryId = model.CostCategoryId,
                Note = model.Note,
                Cost = model.Cost,
            };

            _transactionCostRepository.Add(entity);
            _transactionCostRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = new { id= entity.Id, message = "New cost transaction has been added" },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("delete", Name = "DeleteTransactionCost")]
        public IActionResult DeleteTransactionCost(int id)
        {
            var entity = _transactionCostRepository.GetSingleIncludesAsync(o => o.Id == id).GetAwaiter().GetResult();
            entity.IsDeleted = true;
            _transactionCostRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "Cost Transaction has been deleted",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}