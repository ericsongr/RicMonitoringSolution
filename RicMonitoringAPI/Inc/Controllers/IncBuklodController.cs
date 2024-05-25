using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicCommon.Infrastructure.Extensions;
using RicEntityFramework.Helpers;
using RicEntityFramework.Inc.Interfaces;
using RicEntityFramework.Interfaces;
using RicModel.Inc;
using RicModel.Inc.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.Inc.Controllers
{
    [Route("api/buklod")]
    [ApiController]
    public class IncBuklodController : ControllerBase
    {
        private readonly IIncBuklodRepository _incBuklodRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IMapper _mapper;

        public IncBuklodController(
            IIncBuklodRepository incBuklodRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _incBuklodRepository = incBuklodRepository ?? throw new ArgumentNullException(nameof(incBuklodRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "ViewBuklod")]
        [Route("view")]
        public async Task<IActionResult> View([FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<IncBuklodViewDto>(fields))
            {
                return BadRequest();
            }

            var items = _incBuklodRepository.FindAll();

            if (items == null)
            {
                return NotFound();
            }

            var dataTools = _mapper.Map<IEnumerable<IncBuklodViewDto>>(items.OrderBy(o => o.LastName));

            return Ok(new BaseRestApiModel
            {
                Payload = dataTools.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpGet(Name = "BuklodDetail")]
        [Route("detail/{id}")]
        public async Task<IActionResult> ViewDetail(int id, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<IncBuklodDto>(fields))
            {
                return BadRequest();
            }

            var tools = _incBuklodRepository.Find(id);

            if (tools == null)
            {
                return NotFound();
            }

            var dataTools = _mapper.Map<IncBuklodDto>(tools);

            return Ok(new BaseRestApiModel
            {
                Payload = dataTools.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost(Name = "Post")]
        public IActionResult Post(IncBuklodCreateDto model)
        {
            string message = "New item has been added";
            int id = 0;
            var isStartDateValid = DateTime.TryParse(model.AnniversaryString, out DateTime anniversary);
            var isEndDateValid = DateTime.TryParse(model.BirthdayString, out DateTime birthday);

            if (model.Id > 0)
            {
                var modifiedEntity = _incBuklodRepository.Find(model.Id);

                modifiedEntity.FirstName = model.FirstName;
                modifiedEntity.LastName = model.LastName;
                modifiedEntity.Purok = model.Purok;
                modifiedEntity.Grupo = model.Grupo;
                modifiedEntity.Mobile = model.Mobile;
                modifiedEntity.Anniversary = anniversary;
                modifiedEntity.Birthday = birthday;

                _incBuklodRepository.Update(modifiedEntity);
                id = model.Id;

                message = "Item has been updated";
            }
            else
            {
                var buklod = new IncBuklod();
                buklod.FirstName = model.FirstName;
                buklod.LastName = model.LastName;
                buklod.Purok = model.Purok;
                buklod.Grupo = model.Grupo;
                buklod.Mobile = model.Mobile;
                buklod.Anniversary = anniversary;
                buklod.Birthday = birthday;

                id = _incBuklodRepository.Save(buklod);
            }


            return Ok(new BaseRestApiModel
            {
                Payload = new { id = id, message = message },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }


        [HttpDelete]
        public IActionResult PostDelete(int id)
        {
            string message = "Record has been deleted.";

            var modifiedEntity = _incBuklodRepository.Find(id);
            modifiedEntity.IsDeleted = true;
            _incBuklodRepository.Update(modifiedEntity);

            return Ok(new BaseRestApiModel
            {
                Payload = new { id = id, message = message },
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

    }
}