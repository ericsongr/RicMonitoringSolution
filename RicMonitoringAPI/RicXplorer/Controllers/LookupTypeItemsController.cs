using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Constants;
using RicMonitoringAPI.RicXplorer.Services.Interfaces;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/lookup-type-items")]
    [ApiController]
    public class LookupTypeItemsController : ControllerBase
    {
        private readonly ILookupTypeItemRepository _lookupTypeItemRepository;
        private readonly ITypeHelperService _typeHelperService;


        public LookupTypeItemsController(
            ILookupTypeItemRepository lookupTypeItemRepository,
            ITypeHelperService typeHelperService)
        {
            _lookupTypeItemRepository = lookupTypeItemRepository;
            _typeHelperService = typeHelperService;
        }

        [HttpGet("{lookupTypeName}", Name = "GetLookupItems")]
        public async Task<IActionResult> GetLookupItems(string lookupTypeName, [FromQuery] string fields)
        {
            
            if (!_typeHelperService.TypeHasProperties<LookupTypeItemDto>(fields))
            {
                return BadRequest();
            }

            var lookupTypeItemRepo =
                await _lookupTypeItemRepository.GetSingleIncludesAsync(
                    o => o.Name.ToLower().Trim() == lookupTypeName.ToLower().Trim(), 
                    o => o.LookupTypeItems);
            if (lookupTypeItemRepo == null)
            {
                return NotFound();
            }

            var lookupTypeItems = Mapper.Map<IEnumerable<LookupTypeItemDto>>(lookupTypeItemRepo.LookupTypeItems);

            return Ok(new{ lookupTypeItems = lookupTypeItems.ShapeData(fields) });
        }
    }
}