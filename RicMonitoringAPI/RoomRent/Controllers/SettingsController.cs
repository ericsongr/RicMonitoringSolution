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
using RicCommon.Enumeration;
using RicEntityFramework.Interfaces;
using RicMonitoringAPI.RoomRent.ViewModels;
using RicMonitoringAPI.RoomRent.ViewModels.ApiModels;

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
        private readonly IMapper _mapper;

        public SettingsController(
            ITypeHelperService typeHelperService,
            ISettingRepository settingRepository,
            IMapper mapper)
        {
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
            var accounts = _mapper.Map<IEnumerable<SettingDto>>(settings);
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

        #region IActionResult

        [AllowAnonymous]
        [Route("theme")]
        public IActionResult PostClubTheme(LogoSizeApiModel model)
        {
            string primaryBgColorForApp = _settingRepository.GetValue(SettingNameEnum.PrimaryColorForApp);
            string primaryTextColorForApp = _settingRepository.GetValue(SettingNameEnum.PrimaryTextColorForApp);
            string primaryBgColorForMyAccount = _settingRepository.GetValue(SettingNameEnum.PrimaryColorForMyAccount);
            string primaryTextColorForMyAccount =
                _settingRepository.GetValue(SettingNameEnum.PrimaryTextColorForMyAccount);
            if (string.IsNullOrEmpty(primaryBgColorForMyAccount))
                primaryBgColorForMyAccount = primaryBgColorForApp;
            if (string.IsNullOrEmpty(primaryTextColorForMyAccount))
                primaryBgColorForMyAccount = primaryTextColorForApp;

            string secondaryBgColorForApp = GetSecondaryBackgroundColor();
            string secondaryTextColorForApp = _settingRepository.GetValue(SettingNameEnum.SecondaryTextColorForApp);
            string secondaryBgColorForMyAccount =
                _settingRepository.GetValue(SettingNameEnum.SecondaryColorForMyAccount);
            string secondaryTextColorForMyAccount =
                _settingRepository.GetValue(SettingNameEnum.SecondaryTextColorForMyAccount);
            if (string.IsNullOrEmpty(secondaryBgColorForMyAccount))
                secondaryBgColorForMyAccount = secondaryBgColorForApp;
            if (string.IsNullOrEmpty(secondaryTextColorForMyAccount))
                secondaryTextColorForMyAccount = secondaryTextColorForApp;

            return Ok(new BaseRestApiModel
            {
                Payload = new
                {
                    PrimaryColor = new
                    {
                        BackGround = primaryBgColorForApp,
                        Text = primaryTextColorForApp,
                    },
                    SecondaryColor = new
                    {
                        BackGround = secondaryBgColorForApp,
                        Text = secondaryTextColorForApp,
                    },
                    FourthColor = new
                    {
                        BackGround = _settingRepository.GetValue(SettingNameEnum.FourthColorForApp),
                        Text = "",
                    },
                    FifthColor = new
                    {
                        BackGround = _settingRepository.GetValue(SettingNameEnum.FifthColorForApp),
                        Text = "",
                        Logo = ""
                    },
                }
            });

        }

        #endregion

        
        #region Private Functions

        private string GetSecondaryBackgroundColor()
        {
            var secondaryBackgroundColor = _settingRepository.GetValue(SettingNameEnum.SecondaryColorForApp);
            if (string.IsNullOrEmpty(secondaryBackgroundColor.Trim()) || secondaryBackgroundColor.Trim() == "#")
            {
                return null;
            }
            return secondaryBackgroundColor;
        }

        #endregion
    }
}
