using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RicEntityFramework;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.Parameters;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Model;
using System.Net;
using RicEntityFramework.Services;
using TimeZone = RicModel.RoomRent.TimeZone;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    //[AllowAnonymous]
    [Authorize(Policy = "SuperAndAdmin")]
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ApiBaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IAccountService _accountService;
        private readonly ISmsGatewayService _smsGatewayService;

        public AccountsController(RicDbContext context,
            IAccountRepository accountRepository,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IAccountService accountService,
            ISmsGatewayService smsGatewayService)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _smsGatewayService = smsGatewayService ?? throw new ArgumentNullException(nameof(smsGatewayService));
        }

        [HttpGet("time-zones")]
        public async Task<IActionResult> GetTimeZonesInJson()
        {
            var timeZones = await Task.Run(() =>
                (from TimeZoneInfo tz in _accountService.SetupTimeZones()
                    select new TimeZone
                    {
                        DisplayName = tz.DisplayName,
                        Id = tz.Id
                    }).OrderBy(o => o.DisplayName).ToList()); 
            return Ok(new BaseRestApiModel
            {
                Payload = timeZones
            });
        }

        [HttpGet("{id}", Name = "GetAccount")]
        public async Task<IActionResult> GetAccount(int id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<AccountDto>(fields))
            {
                return BadRequest();
            }

            var roomFromRepo = await _accountRepository.GetSingleAsync(o => o.Id == id);
            if (roomFromRepo == null)
            {
                return NotFound();
            }

            var account = Mapper.Map<AccountDto>(roomFromRepo);

            return Ok(new BaseRestApiModel
            {
                Payload = account.ShapeData(fields)
            });
        }

        // GET: api/Rooms
        [HttpGet(Name = "GetAccounts")]
        public IActionResult GetAccounts([FromQuery] AccountResourceParameters accountResourceParameters)
        {
            if (!_typeHelperService.TypeHasProperties<AccountDto>
                (accountResourceParameters.Fields))
            {
                return BadRequest();
            }

            var accountFromRepo = _accountRepository.GetAccounts(accountResourceParameters);
            var accounts = Mapper.Map<IEnumerable<AccountDto>>(accountFromRepo).ToList();
            
            //if there's no selected default account, then set the first account as default selected account avoid error in RicMoniApp
            if (!accounts.Any(o => o.IsSelected))
                accounts.First().IsSelected = true;

            var result = accounts.AsEnumerable().ShapeData(accountResourceParameters.Fields);

            return Ok(new BaseRestApiModel
            {
                Payload = result,
                Errors = new List<BaseErrorModel>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [Authorize(Roles = "Superuser")]
        [HttpPost()]
        public IActionResult CreateAccount([FromBody] AccountForCreateDto account)
        {
            if (account == null)
            {
                return NotFound();
            }

            try
            {
                if (account.IsSelected)
                    UpdateToUnSelectedAccount();

                var roomEntity = Mapper.Map<Account>(account);

                _accountRepository.Add(roomEntity);
                _accountRepository.Commit();

                //null to avoid error
                roomEntity.AuditAccounts = null;

                return Ok(new BaseRestApiModel
                {
                    Payload = roomEntity.Id,
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            catch (System.Exception ex)
            {
                return Ok(HandleApiException(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Superuser")]
        [HttpPut("{id}", Name = "UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountForUpdateDto account)
        {
            if (account == null)
            {
                return NotFound();
            }

            try
            {
                var accountEntity = await _accountRepository.GetSingleAsync(o => o.Id == id);
                if (accountEntity == null)
                {
                    return NotFound();
                }

                if (!accountEntity.IsSelected && account.IsSelected)
                    UpdateToUnSelectedAccount();

                accountEntity.Name = account.Name;
                accountEntity.Timezone = account.Timezone;
                accountEntity.IsActive = account.IsActive;
                accountEntity.Street = account.Street;
                accountEntity.SubUrb = account.SubUrb;
                accountEntity.State = account.State;
                accountEntity.PostalCode = account.PostalCode;
                accountEntity.Email = account.Email;
                accountEntity.PhoneNumber = account.PhoneNumber;
                accountEntity.WebsiteUrl = account.WebsiteUrl;
                accountEntity.FacebookUrl = account.FacebookUrl;
                accountEntity.AddressLine1 = account.AddressLine1;
                accountEntity.City = account.City;
                accountEntity.DialingCode = account.DialingCode;
                accountEntity.BusinessOwnerName = account.BusinessOwnerName;
                accountEntity.BusinessOwnerPhoneNumber = account.BusinessOwnerPhoneNumber;
                accountEntity.BusinessOwnerEmail = account.BusinessOwnerEmail;
                accountEntity.GeoCoordinates = account.GeoCoordinates;
                accountEntity.CompanyFeeFailedPaymentCount = account.CompanyFeeFailedPaymentCount;
                accountEntity.PaymentIssueSuspensionDate = account.PaymentIssueSuspensionDate;
                accountEntity.IsSelected = account.IsSelected;

                _accountRepository.Update(accountEntity);
                _accountRepository.Commit();

                //null to avoid error
                accountEntity.AuditAccounts = null;

                return Ok(new BaseRestApiModel
                {
                    Payload = accountEntity.Id,
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            catch (System.Exception ex)
            {
                return Ok(HandleApiException(ex.Message + " InnerException: " + ex.InnerException.Message, HttpStatusCode.BadRequest));
            }
        }

        private void UpdateToUnSelectedAccount()
        {
            var account = _accountRepository.GetSingleAsync(o => o.IsSelected).GetAwaiter().GetResult();
            if (account != null)
            {
                account.IsSelected = false;
                _accountRepository.Update(account);
                _accountRepository.Commit();
            }
        }

        private string CreateAccountResourceUri(
                            AccountResourceParameters accountResourceParamaters,
                            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAccounts",
                        new
                        {
                            fields = accountResourceParamaters.Fields,
                            orderBy = accountResourceParamaters.OrderBy,
                            searchQuery = accountResourceParamaters.SearchQuery,
                            pageNumber = accountResourceParamaters.PageNumber - 1,
                            pageSize = accountResourceParamaters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAccounts",
                        new
                        {
                            fields = accountResourceParamaters.Fields,
                            orderBy = accountResourceParamaters.OrderBy,
                            searchQuery = accountResourceParamaters.SearchQuery,
                            pageNumber = accountResourceParamaters.PageNumber + 1,
                            pageSize = accountResourceParamaters.PageSize
                        });

                default:
                    return _urlHelper.Link("GetAccounts",
                            new
                            {
                                fields = accountResourceParamaters.Fields,
                                orderBy = accountResourceParamaters.OrderBy,
                                searchQuery = accountResourceParamaters.SearchQuery,
                                pageNumber = accountResourceParamaters.PageNumber,
                                pageSize = accountResourceParamaters.PageSize
                            });
            }
        }

        [Authorize(Roles = "Superuser")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Room>> DeleteAccount(int id)
        {
            var accountEntity = await _accountRepository.GetSingleAsync(o => o.Id == id);
            if (accountEntity == null)
            {
                return NotFound();
            }

            _accountRepository.Delete(accountEntity);
            _accountRepository.Commit();

            return Ok(new { message = "Account successfully deleted." });
        }

    }
}
