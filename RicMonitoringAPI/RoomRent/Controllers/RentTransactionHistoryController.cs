using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.Parameters;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    //[Authorize(Policy = "Superuser")]
    [AllowAnonymous]
    [Route("api/rent-transaction-history")]
    [ApiController]
    public class RentTransactionHistoryController : ControllerBase
    {
        private readonly RicDbContext _context;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IRentTransactionHistoryPropertyMappingService _rentTransactionHistoryPropertyMappingService;

        public RentTransactionHistoryController(
            RicDbContext context,
            ITypeHelperService typeHelperService,
            IRentTransactionHistoryPropertyMappingService rentTransactionHistoryPropertyMappingService,
            IRentTransactionHistoryRepository rentTransactionHistoryRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _rentTransactionHistoryPropertyMappingService = rentTransactionHistoryPropertyMappingService ?? throw new ArgumentNullException(nameof(rentTransactionHistoryPropertyMappingService));
        }

        [HttpGet(Name = "GetHistories")]
        public IActionResult GetAll([FromQuery] RentTransactionHistoryParameters parameters)
        {
            if (!_rentTransactionHistoryPropertyMappingService
                .ValidMappingExistsFor<RentTransactionHistoryDto, RentTransaction>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<RentTransactionHistoryDto>(parameters.Fields))
            {
                return BadRequest();
            }

            var rentTransactionHistories = _context.RentTransactions
                .Where(o => o.RenterId == parameters.Id)
                .Include(o => o.Room)
                .Include(o => o.RentTransactionDetails)
                .ApplySort(parameters.OrderBy,
                    _rentTransactionHistoryPropertyMappingService
                        .GetPropertyMapping<RentTransactionHistoryDto, RentTransaction>());
                

            var histories = Mapper.Map<IEnumerable<RentTransactionHistoryDto>>(rentTransactionHistories);
            
            return Ok(histories.ShapeData(parameters.Fields));
        }

    }
}