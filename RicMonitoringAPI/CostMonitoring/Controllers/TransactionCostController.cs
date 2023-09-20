using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        private readonly IMapper _mapper;


        public TransactionCostController(
            ICostItemRepository costItemRepository,
            ITransactionCostRepository transactionCostRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _costItemRepository = costItemRepository ?? throw new ArgumentNullException(nameof(costItemRepository));
            _transactionCostRepository = transactionCostRepository ?? throw new ArgumentNullException(nameof(transactionCostRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "TransactionCosts")]
        [Route("list")]
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
            }

            if (costCategoryId > 0)
                transactionCosts = transactionCosts.Where(o => o.CostCategoryId == costCategoryId);

            if (transactionCosts == null)
            {
                return NotFound();
            }

            var data = _mapper.Map<IEnumerable<TransactionCostDto>>(transactionCosts.OrderByDescending(o => o.TransactionDate));

            return Ok(new BaseRestApiModel
            {
                Payload = data.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpGet(Name = "Graph")]
        [Route("graph")]
        public async Task<IActionResult> Graph(string startDate, string endDate, bool isFilterByCurrentMonth = true)
        {


            var transactionCosts = _transactionCostRepository
                .FindAll(o => o.CostItem, o => o.CostCategory);

            var items = _costItemRepository.FindAll().OrderBy(o => o.Name).ToList();

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

            }

            if (transactionCosts == null)
            {
                return NotFound();
            }

            var summaries = transactionCosts.GroupBy(x => x.CostCategory.Description,
                (category, categoryItems) => new TransactionCostSummaryDto
                {
                    CategoryId = categoryItems.FirstOrDefault().CostCategoryId,
                    Category = category,
                    Total = categoryItems.Sum(i => i.Cost),
                    Count = categoryItems.Count(),
                    Items = categoryItems.GroupBy(s => s.CostItem.Name,
                        (itemName, items) => new Item
                        {
                            ItemName = itemName,
                            Total = items.Sum(s => s.Cost),
                            Count = items.Count(),
                            BackgroundColor = items.FirstOrDefault().CostItem.BackgroundColor,
                            ItemId = items.FirstOrDefault().CostItem.Id
                        }).ToList()
                }).OrderBy(o => o.Category).ToList();


            summaries.ForEach(summary =>
            {
                var itemIds = summary.Items.Select(o => o.ItemId).ToList();
                var itemsNotExists = items.Where(o => !itemIds.Contains(o.Id))
                    .Select(o => new Item
                    {
                        Count = 0,
                        ItemId = o.Id,
                        ItemName = o.Name,
                        BackgroundColor = o.BackgroundColor
                    }).ToList();
                if (itemsNotExists.Any())
                    summary.Items.AddRange(itemsNotExists);

                summary.Items = summary.Items.OrderBy(o => o.ItemName).ToList();
            });
            
            //put in the list
            var itemsOutput = new List<Item>();
            items.ForEach(item =>
            {
                summaries.ForEach(summary =>
                {
                    var itemOutput = summary.Items.Where(o => o.ItemId == item.Id).ToList();
                    itemsOutput.AddRange(itemOutput);
                });
            });

            // then group
            var data = itemsOutput.GroupBy(o => o.ItemName, (name, itemList) =>
                new ItemOutput
                {
                    Name = name,
                    BackgroundColor = itemList.FirstOrDefault().BackgroundColor,
                    Total = itemList.Select(o => o.Total).ToList()
                }).ToList();


            var summaryList = new List<TransactionCostSummaryOutput>();
            summaries.ForEach(summary =>
            {
                var summaryOutput = new TransactionCostSummaryOutput
                {
                    CategoryId = summary.CategoryId,
                    Category = summary.CategoryAndTotal,
                    Total = summary.Total,
                    Count = summary.Count
                };

                summaryList.Add(summaryOutput);
            });

            var graphDataOutput = new GraphDataOutput
            {
                Categories = summaryList,
                Items = data
            };

            return Ok(new BaseRestApiModel
            {
                Payload = graphDataOutput,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost(Name = "AddTransactionCost")]
        public IActionResult AddTransactionCost(TransactionCostDto model)
        {
            string message = "New cost transaction has been added";
            DateTime.TryParse(model.TransactionDate, out DateTime transDate);
            var entity = new TransactionCost
            {
                TransactionDate = transDate,
                CostItemId = model.CostItemId,
                CostCategoryId = model.CostCategoryId,
                Note = model.Note,
                Cost = model.Cost,
            };

            if (model.Id > 0)
            {
                var modifiedEntity = _transactionCostRepository.GetSingleAsync(o => o.Id == model.Id).GetAwaiter().GetResult();

                modifiedEntity.TransactionDate = entity.TransactionDate;
                modifiedEntity.CostItemId = entity.CostItemId;
                modifiedEntity.CostCategoryId = entity.CostCategoryId;
                modifiedEntity.Note = entity.Note;
                modifiedEntity.Cost = entity.Cost;

                _transactionCostRepository.Update(modifiedEntity);
                message = "Cost transaction has been updated";
            }
            else
            {
                _transactionCostRepository.Add(entity);
            }

            _transactionCostRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = new { id = entity.Id, message = message },
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