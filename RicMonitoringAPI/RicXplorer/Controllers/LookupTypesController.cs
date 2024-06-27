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
using RicModel.RoomRent;
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
        public async Task<IActionResult> GetLookupItems(string lookUps, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<LookupTypeDto>(fields))
            {
                return BadRequest();
            }

            IEnumerable<LookupType> lookupTypes = null;
            if (string.IsNullOrEmpty(lookUps))
            {
                lookupTypes = _lookupTypeRepository.FindAll();
            }
            else
            {
                var lookUpTypeIds = lookUps.Split(',').Select(int.Parse).ToList();
                lookupTypes = _lookupTypeRepository.FindBy(o => lookUpTypeIds.Contains(o.Id));
            }
            
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

        [HttpPost("add")]
        public IActionResult AddLookup(LookupTypeDto model)
        {
            string message = "New lookup has been saved.";


            var entity = new LookupType
            {
                Name = model.Name,
            };

            _lookupTypeRepository.Add(entity);
            _lookupTypeRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = new { id = entity.Id, message },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("update")]
        public IActionResult UpdateLookup(LookupTypeDto model)
        {
            var entity = _lookupTypeRepository.FindBy(o => o.Id == model.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.Name = model.Name;

                _lookupTypeRepository.Update(entity);
                _lookupTypeRepository.Commit();
            }

            return Ok(new BaseRestApiModel
            {
                Payload = new { id = entity.Id, message = "Lookup has been updated." },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpDelete]
        public ActionResult DeleteLookup(int id)
        {
            var entity = _lookupTypeRepository.FindBy(o => o.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.IsDeleted = true;
                _lookupTypeRepository.Update(entity);
                _lookupTypeRepository.Commit();
            }

            return Ok(new BaseRestApiModel
            {
                Payload = new { id = entity.Id, message = "Lookup Item successfully deleted." },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}