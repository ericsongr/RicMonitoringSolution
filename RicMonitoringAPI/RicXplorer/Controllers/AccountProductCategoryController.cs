using System;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.RicXplorer.Interfaces;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/account-product-category")]
    [ApiController]
    public class AccountProductCategoryController : ControllerBase
    {
        private readonly IAccountProductCategoryRepository _accountProductCategoryRepository;


        public AccountProductCategoryController(
            IAccountProductCategoryRepository accountProductCategoryRepository)
        {
            _accountProductCategoryRepository = accountProductCategoryRepository ?? throw new ArgumentNullException(nameof(accountProductCategoryRepository));
        }

    }
}