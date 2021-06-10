using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RicEntityFramework.Helpers;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Model;
using System.Net;
using System.Threading.Tasks;
using RicEntityFramework.Interfaces;
using RicMonitoringAPI.RoomRent.ViewModels;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [AllowAnonymous]
    [Authorize(Policy = "SuperAndAdmin")]
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ApiBaseController
    {
        private readonly ITypeHelperService _typeHelperService;
        private readonly ISettingRepository _settingRepository;

        public SettingsController(
            ITypeHelperService typeHelperService,
            ISettingRepository settingRepository)
        {
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }

        // GET: api/Rooms
        [HttpGet(Name = "GetSettings")]
        public IActionResult GetSettings(string fields)
        {
            if (!_typeHelperService.TypeHasProperties<SettingDto>(fields))
            {
                return BadRequest();
            }

            var settings = _settingRepository.GetAll();
            var accounts = Mapper.Map<IEnumerable<SettingDto>>(settings);
            var result = accounts.ShapeData(fields);


            return Ok(new BaseRestApiModel
            {
                Payload = result,
                Errors = new List<BaseErrorModel>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPut("{id}", Name = "UpdateSetting")]
        public async Task<IActionResult> UpdateSetting(int id, [FromBody] SettingModel setting)
        {
            if (setting == null)
                return NotFound();

            try
            {
                var settingEntity = await _settingRepository.GetSingleAsync(o => o.Key == setting.Key);
                if (settingEntity == null)
                    return NotFound();

                settingEntity.Value = setting.Value;

                _settingRepository.Update(settingEntity);
                _settingRepository.Commit();

                return Ok(new BaseRestApiModel
                {
                    Payload = settingEntity.Id,
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                return Ok(HandleApiException(ex.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
