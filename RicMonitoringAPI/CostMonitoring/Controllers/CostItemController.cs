using System;
using System.Collections.Generic;
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
    [Route("api/cost-monitoring")]
    [ApiController]
    public class CostItemController : ControllerBase
    {
        private readonly ICostItemRepository _costItemRepository;
        private readonly ITypeHelperService _typeHelperService;


        public CostItemController(
            ICostItemRepository costItemRepository,
            ITypeHelperService typeHelperService)
        {
            _costItemRepository = costItemRepository ?? throw new ArgumentNullException(nameof(costItemRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
        }

        [HttpGet(Name = "GetCostItems")]
        public async Task<IActionResult> GetCostItems([FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<CostItemDto>(fields))
            {
                return BadRequest();
            }

            var costItems = _costItemRepository.FindAll();
            if (costItems == null)
            {
                return NotFound();
            }

            var lookupTypeItems = Mapper.Map<IEnumerable<CostItemDto>>(costItems);

            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost(Name = "AddCostItem")]
        public IActionResult AddCostItem(CostItemDto model)
        {
            var entity = new CostItem
            {
                Name = model.Name,
            };

            _costItemRepository.Add(entity);
            _costItemRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "New cost item has been added",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("delete", Name = "DeleteCostItem")]
        public IActionResult DeleteCostItem(int id)
        {
            var entity = _costItemRepository.GetSingleIncludesAsync(o => o.Id == id).GetAwaiter().GetResult();
            entity.IsDeleted = true;
            _costItemRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "Cost item has been deleted",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}