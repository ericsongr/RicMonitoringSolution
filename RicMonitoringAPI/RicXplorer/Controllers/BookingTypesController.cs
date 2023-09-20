using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer.Dtos;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/booking-types")]
    [ApiController]
    public class BookingTypesController : ControllerBase
    {
        private readonly IBookingTypeRepository _bookingTypeRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IMapper _mapper;


        public BookingTypesController(
            IBookingTypeRepository bookingTypeRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _bookingTypeRepository = bookingTypeRepository ?? throw new ArgumentNullException(nameof(bookingTypeRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetActiveBookingTypes")]
        public IActionResult GetActiveBookingTypes([FromQuery] string fields)
        {
            
            if (!_typeHelperService.TypeHasProperties<BookingTypeDto>(fields))
            {
                return BadRequest();
            }

            var bookingTypes = _bookingTypeRepository
                .FindAll().Where(o => o.IsActive)
                .Include(o => o.AccountProduct)
                .Include(o => o.BookingTypeInclusions)
                .ThenInclude(o => o.LookupTypeItem)
                .Include(o => o.BookingTypeImages)
                .ToList();
            
            if (!bookingTypes.Any())
            {
                return NotFound();
            }

            var bookingTypeDtos = _mapper.Map<IEnumerable<BookingTypeDto>>(bookingTypes).ToList();

            return Ok(new{payload = bookingTypeDtos });
        }
    }
}