using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/lookup-types")]
    [ApiController]
    public class LookupTypesController : ControllerBase
    {
        private readonly ILookupTypeRepository _lookupTypeRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IMapper _mapper;


        public LookupTypesController(
            ILookupTypeRepository lookupTypeRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _lookupTypeRepository = lookupTypeRepository ?? throw new ArgumentNullException(nameof(lookupTypeRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetLookups")]
        public async Task<IActionResult> GetLookupItems([FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<LookupTypeDto>(fields))
            {
                return BadRequest();
            }

            var lookUpTypes = new List<string>
            {
                "Ages",
                "Booking Type Inclusions",
                "Cost Categories",
                "Inventory Tool Action",
                "Inventory Tool Status",
            };

            var lookupTypes = _lookupTypeRepository.FindBy(o => lookUpTypes.Contains(o.Name));
            if (lookupTypes == null)
            {
                return NotFound();
            }

            var lookupTypeItems = _mapper.Map<IEnumerable<LookupTypeCategoryDto>>(lookupTypes);

            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        //[HttpPost()]
        //public IActionResult AddCategory(LookupTypeItemDto model)
        //{
        //    string message = string.Empty;
        //    var entity = new LookupTypeItem
        //    {
        //        Description = model.Description,
        //        IsActive = true,
        //        LookupTypeId = model.LookupTypeId
        //    };
        //    _lookupTypeItemRepository.Add(entity);
        //    _lookupTypeItemRepository.Commit();

        //    var lookUp = _lookupTypeRepository.FindBy(o => o.Id == model.LookupTypeId).FirstOrDefault(); // should always exist

        //    switch (lookUp.Name)
        //    {
        //        case LookupTypeConstant.CostCategories:
        //            message = "New cost category has been saved.";
        //            break;
        //        default:
        //            message = "New look up item has been saved.";
        //            break;
        //    }

        //    return Ok(new BaseRestApiModel
        //    {
        //        Payload = new { id = entity.Id, message },
        //        Errors = new List<BaseError>(),
        //        StatusCode = (int)HttpStatusCode.OK
        //    });
        //}
    }
}