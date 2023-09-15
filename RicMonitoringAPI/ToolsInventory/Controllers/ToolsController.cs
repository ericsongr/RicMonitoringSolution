using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicCommon.Constants;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.Services;
using RicEntityFramework.ToolsInventory.Interfaces;
using RicModel.CostMonitoring.Dtos;
using RicModel.ToolsInventory;
using RicModel.ToolsInventory.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.ToolsInventory.Controllers
{
    [Route("api/tools")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IToolRepository _toolRepository;
        private readonly IToolInventoryRepository _toolInventoryRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IImageService _imageService;


        public ToolsController(
            IToolRepository toolRepository,
            IToolInventoryRepository toolInventoryRepository,
            ITypeHelperService typeHelperService,
            IImageService imageService)
        {
            _toolRepository = toolRepository ?? throw new ArgumentNullException(nameof(toolRepository));
            _toolInventoryRepository = toolInventoryRepository ?? throw new ArgumentNullException(nameof(toolInventoryRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }


        [HttpGet(Name = "View")]
        [Route("view")]
        public async Task<IActionResult> View(string startDate, string endDate, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<ToolViewDto>(fields))
            {
                return BadRequest();
            }

            var tools = _toolRepository.FindAll(o => o.ToolsInventory);

            var isStartDateValid = DateTime.TryParse(startDate, out DateTime startDateOut);
            var isEndDateValid = DateTime.TryParse(endDate, out DateTime endDateOut);
            if (!isStartDateValid)
                return BadRequest("Invalid start date");
            if (!isEndDateValid)
                return BadRequest("Invalid end date");

            tools = tools.Where(o => o.CreatedDateTimeUtc.Date >= startDateOut && o.CreatedDateTimeUtc.Date <= endDateOut);

            if (tools == null)
            {
                return NotFound();
            }

            var data = Mapper.Map<IEnumerable<ToolViewDto>>(tools.OrderByDescending(o => o.CreatedDateTimeUtc));

            return Ok(new BaseRestApiModel
            {
                Payload = data.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost(Name = "PostTool")]
        public IActionResult PostTool(CreateNewToolDto model)
        {
            string message = "New tool has been added";

            var fileNames = new List<string>();
            model.FilesInBase64.ForEach(base64 =>
            {
                var filename = _imageService.Upload(base64, FolderConstant.InventoryToolsImage);
                fileNames.Add(filename);
            });
            var fileNamesInCommaDelimited = string.Join(",", fileNames);

            var tool = new Tool
            {
                Name = model.Name,
                Description = model.Description,
                PowerTool = model.PowerTool,
                CreatedBy = "TODO"
            };

            if (model.Id > 0)
            {
                var modifiedEntity = _toolRepository.GetSingleAsync(o => o.Id == model.Id).GetAwaiter().GetResult();

                modifiedEntity.Name = model.Name;
                modifiedEntity.Description = model.Description;
                modifiedEntity.PowerTool = model.PowerTool;

                _toolRepository.Update(modifiedEntity);
                _toolRepository.Commit();

                message = "Cost tool has been updated";
            }
            else
            {
                tool.CreatedDateTimeUtc = DateTime.UtcNow;

                _toolRepository.Add(tool);
                _toolRepository.Commit();

                var toolInventory = new ToolInventory
                {
                    ToolId = tool.Id,
                    Images = fileNamesInCommaDelimited,
                    Status = ToolStatusConstant.Working,
                    Action = ToolActionConstant.NewlyAdded,
                    InventoryDateTimeUtc = DateTime.UtcNow,
                    CreatedDateTimeUtc = DateTime.UtcNow
                };

                _toolInventoryRepository.Add(toolInventory);
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