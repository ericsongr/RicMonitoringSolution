using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Constants;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/lookup-type-items")]
    [ApiController]
    public class LookupTypeItemsController : ControllerBase
    {
        private readonly ILookupTypeRepository _lookupTypeRepository;
        private readonly ILookupTypeItemRepository _lookupTypeItemRepository;
        private readonly ITypeHelperService _typeHelperService;


        public LookupTypeItemsController(
            ILookupTypeRepository lookupTypeRepository,
            ILookupTypeItemRepository lookupTypeItemRepository,
            ITypeHelperService typeHelperService)
        {
            _lookupTypeRepository = lookupTypeRepository ?? throw new ArgumentNullException(nameof(lookupTypeRepository));
            _lookupTypeItemRepository = lookupTypeItemRepository ?? throw new ArgumentNullException(nameof(lookupTypeItemRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
        }

        [HttpGet("{lookupTypeName}", Name = "GetLookupItems")]
        public async Task<IActionResult> GetLookupItems(string lookupTypeName, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<LookupTypeItemDto>(fields))
            {
                return BadRequest();
            }

            var lookupTypeItemRepo =
                await _lookupTypeRepository.GetSingleIncludesAsync(
                    o => o.Name.ToLower().Trim() == lookupTypeName.ToLower().Trim(),
                    o => o.LookupTypeItems);
            if (lookupTypeItemRepo == null)
            {
                return NotFound();
            }

            var lookupTypeItems = Mapper.Map<IEnumerable<LookupTypeItemDto>>(lookupTypeItemRepo.LookupTypeItems);

            return Ok(new { lookupTypeItems = lookupTypeItems.ShapeData(fields) });
            
        }

        [HttpPost()]
        public IActionResult AddCategory(LookupTypeItemDto model)
        {
            string message = string.Empty;
            var entity = new LookupTypeItem
            {
                Description = model.Description,
                IsActive = true,
                LookupTypeId = model.LookupTypeId
            };
            _lookupTypeItemRepository.Add(entity);
            _lookupTypeItemRepository.Commit();

            var lookUp = _lookupTypeRepository.FindBy(o => o.Id == model.LookupTypeId).FirstOrDefault(); // should always exist

            switch (lookUp.Name)
            {
                case LookupTypeConstant.CostCategories:
                    message = "New cost category has been saved.";
                    break;
                default:
                    message = "New look up item has been saved.";
                    break;
            }

            return Ok(new BaseRestApiModel
            {
                Payload = message,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}