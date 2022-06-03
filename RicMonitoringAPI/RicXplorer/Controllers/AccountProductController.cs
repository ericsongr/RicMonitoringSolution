using System;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.RicXplorer.Interfaces;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/account-product")]
    [ApiController]
    public class AccountProductController : ControllerBase
    {
        private readonly IAccountProductCategoryRepository _accountProductCategoryRepository;


        public AccountProductController(
            IAccountProductCategoryRepository accountProductCategoryRepository)
        {
            _accountProductCategoryRepository = accountProductCategoryRepository ?? throw new ArgumentNullException(nameof(accountProductCategoryRepository));
        }

    }
}