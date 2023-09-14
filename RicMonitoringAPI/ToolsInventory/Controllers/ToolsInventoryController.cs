using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Interfaces;
using RicEntityFramework.ToolsInventory.Interfaces;
using RicModel.ToolsInventory;
using RicModel.ToolsInventory.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.ToolsInventory.Controllers
{
    [Route("api/tools-inventory")]
    [ApiController]
    public class ToolsInventoryController : ControllerBase
    {
        private readonly IToolInventoryRepository _toolInventoryRepository;
        private readonly ITypeHelperService _typeHelperService;


        public ToolsInventoryController(
            IToolInventoryRepository toolInventoryRepository,
            ITypeHelperService typeHelperService)
        {
            _toolInventoryRepository = toolInventoryRepository ?? throw new ArgumentNullException(nameof(toolInventoryRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
        }


        //[HttpGet(Name = "View")]
        //[Route("view")]
        //public async Task<IActionResult> View(string startDate, string endDate, [FromQuery] string fields)
        //{

        //    if (!_typeHelperService.TypeHasProperties<ToolViewDto>(fields))
        //    {
        //        return BadRequest();
        //    }

        //    var tools = _toolRepository.FindAll(o => o.ToolsInventory);

        //    var isStartDateValid = DateTime.TryParse(startDate, out DateTime startDateOut);
        //    var isEndDateValid = DateTime.TryParse(endDate, out DateTime endDateOut);
        //    if (!isStartDateValid)
        //        return BadRequest("Invalid start date");
        //    if (!isEndDateValid)
        //        return BadRequest("Invalid end date");

        //    tools = tools.Where(o => o.CreatedDateTimeUtc.Date >= startDateOut && o.CreatedDateTimeUtc.Date <= endDateOut);

        //    if (tools == null)
        //    {
        //        return NotFound();
        //    }

        //    var data = Mapper.Map<IEnumerable<ToolViewDto>>(tools.OrderByDescending(o => o.CreatedDateTimeUtc));

        //    return Ok(new BaseRestApiModel
        //    {
        //        Payload = data.ShapeData(fields),
        //        Errors = new List<BaseError>(),
        //        StatusCode = (int)HttpStatusCode.OK
        //    });

        //}

        [HttpPost(Name = "PostToolInventory")]
        public IActionResult PostToolInventory(ToolInventoryDto model)
        {
            string message = "Tool inventory has been added";

            DateTime.TryParse(model.InventoryDateTime, out DateTime inventoryDateTimeOutput);

            var tool = new ToolInventory()
            { 
               ToolId = model.ToolId,
               InventoryDateTimeUtc = inventoryDateTimeOutput,
               Action = model.Action,// ToolActionConstant.RunTest,
               Status = model.Status // ToolStatusConstant.StillWorking
            };

            if (model.Id > 0)
            {
                var modifiedEntity = _toolInventoryRepository.GetSingleAsync(o => o.Id == model.Id).GetAwaiter().GetResult();

                modifiedEntity.InventoryDateTimeUtc = tool.InventoryDateTimeUtc;
                modifiedEntity.Action = tool.Action;
                modifiedEntity.Status = tool.Status;

                _toolInventoryRepository.Update(modifiedEntity);
                _toolInventoryRepository.Commit();

                tool.Id = modifiedEntity.Id;

                message = "Tool inventory has been updated";
            }
            else
            {

                tool.CreatedDateTimeUtc = DateTime.UtcNow;

                _toolInventoryRepository.Add(tool);
                _toolInventoryRepository.Commit();
                
            }

           
            return Ok(new BaseRestApiModel
            {
                Payload = new { id = tool.Id, message = message },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
        
    }
}