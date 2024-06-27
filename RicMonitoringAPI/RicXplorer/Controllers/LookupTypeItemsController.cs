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
using static Azure.Core.HttpHeader;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/lookup-type-items")]
    [ApiController]
    public class LookupTypeItemsController : ControllerBase
    {
        private readonly ILookupTypeRepository _lookupTypeRepository;
        private readonly ILookupTypeItemRepository _lookupTypeItemRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IMapper _mapper;


        public LookupTypeItemsController(
            ILookupTypeRepository lookupTypeRepository,
            ILookupTypeItemRepository lookupTypeItemRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _lookupTypeRepository = lookupTypeRepository ?? throw new ArgumentNullException(nameof(lookupTypeRepository));
            _lookupTypeItemRepository = lookupTypeItemRepository ?? throw new ArgumentNullException(nameof(lookupTypeItemRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

            var lookupTypeItems = _mapper.Map<IEnumerable<LookupTypeItemDto>>(lookupTypeItemRepo.LookupTypeItems.Where(o => !o.IsDeleted).OrderBy(o => o.Description));

            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpGet("by-id/{id}", Name = "GetLookupByIdItems")]
        public async Task<IActionResult> GetLookupByIdItems(int id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<LookupTypeItemDto>(fields))
            {
                return BadRequest();
            }

            var lookupTypeItemRepo =
                await _lookupTypeRepository.GetSingleIncludesAsync(
                    o => o.Id == id,
                    o => o.LookupTypeItems);
            if (lookupTypeItemRepo == null)
            {
                return NotFound();
            }

            var lookupTypeItems = _mapper.Map<IEnumerable<LookupTypeItemDto>>(lookupTypeItemRepo.LookupTypeItems.Where(o => !o.IsDeleted).OrderBy(o => o.Description));

            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost("add")]
        public IActionResult AddCategory(LookupTypeItemDto model)
        {
            string message = string.Empty;

           
            var entity = new LookupTypeItem
            {
                Description = model.Description,
                IsActive = true,
                Notes = model.Notes,
            };

            if (model.LookupTypeId > 0)
            {
                entity.LookupTypeId = model.LookupTypeId;
            }
            else
            {
                var lookupTypeItemRepo =
                    _lookupTypeRepository.GetSingleAsync(
                        o => o.Name.ToLower().Trim() == model.LookupTypeName.ToLower().Trim()).GetAwaiter().GetResult();

                entity.LookupTypeId = lookupTypeItemRepo.Id;
                model.LookupTypeId = lookupTypeItemRepo.Id;
            }

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
                Payload = new { id = entity.Id, message },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("update")]
        public IActionResult UpdateCategory(LookupTypeItemDto model)
        {
            var entity = _lookupTypeItemRepository.FindBy(o => o.Id == model.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.Description = model.Description;
                entity.Notes = model.Notes;
                _lookupTypeItemRepository.Update(entity);
                _lookupTypeItemRepository.Commit();
            }

            return Ok(new BaseRestApiModel
            {
                Payload = new { id = entity.Id, message = "Category has been updated."},
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpDelete]
        public ActionResult DeleteCategory(int id)
        {
            var entity = _lookupTypeItemRepository.FindBy(o => o.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.IsDeleted = true;
                _lookupTypeItemRepository.Update(entity);
                _lookupTypeItemRepository.Commit();
            }
            
            return Ok(new BaseRestApiModel
            {
                Payload = new { id = entity.Id, message = "Look Up successfully deleted." },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}