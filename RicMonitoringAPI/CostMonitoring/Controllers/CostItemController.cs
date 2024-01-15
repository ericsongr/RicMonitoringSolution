using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicCommon.Infrastructure;
using RicEntityFramework.CostMonitoring.Interfaces;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicModel.CostMonitoring;
using RicModel.CostMonitoring.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.CostMonitoring.Controllers
{
    [Route("api/cost-monitoring-item")]
    [ApiController]
    public class CostItemController : ControllerBase
    {
        private readonly ICostItemRepository _costItemRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IMapper _mapper;


        public CostItemController(
            ICostItemRepository costItemRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _costItemRepository = costItemRepository ?? throw new ArgumentNullException(nameof(costItemRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetCostItems")]
        public async Task<IActionResult> GetCostItems([FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<CostItemDto>(fields))
            {
                return BadRequest();
            }

            var costItems = _costItemRepository.FindAll().OrderBy(o => o.Name);
            if (costItems == null)
            {
                return NotFound();
            }

            var lookupTypeItems = _mapper.Map<IEnumerable<CostItemDto>>(costItems);

            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost("add", Name = "AddCostItem")]
        public IActionResult AddCostItem(CostItemDto model)
        {
            var entity = new CostItem
            {
                Name = model.Name,
                BackgroundColor = RandomHexColor.Generate()
            };

            _costItemRepository.Add(entity);
            _costItemRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = new {id = entity.Id, message = "New cost item has been added" },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("update", Name = "UpdateCostItem")]
        public IActionResult UpdateCostItem(CostItemDto model)
        {
            var entity = _costItemRepository.FindBy(o => o.Id == model.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.BackgroundColor = model.BackgroundColor;
                _costItemRepository.Update(entity);
                _costItemRepository.Commit();
            }

            return Ok(new BaseRestApiModel
            {
                Payload = new { id = entity.Id, message = "Cost item has been updated." },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpDelete]
        public IActionResult DeleteCostItem(int id)
        {
            var entity = _costItemRepository.GetSingleIncludesAsync(o => o.Id == id).GetAwaiter().GetResult();
            if (entity != null)
            {
                entity.IsDeleted = true;
                _costItemRepository.Commit();
            }

            return Ok(new BaseRestApiModel
            {
                Payload = "Cost item has been deleted",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}