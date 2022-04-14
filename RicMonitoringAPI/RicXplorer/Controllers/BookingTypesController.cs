using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RoomRent.Dtos;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/booking-types")]
    [ApiController]
    public class BookingTypesController : ControllerBase
    {
        private readonly IBookingTypeRepository _bookingTypeRepository;
        private readonly ITypeHelperService _typeHelperService;


        public BookingTypesController(
            IBookingTypeRepository bookingTypeRepository,
            ITypeHelperService typeHelperService)
        {
            _bookingTypeRepository = bookingTypeRepository ?? throw new ArgumentNullException(nameof(bookingTypeRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetActiveBookingTypes")]
        public IActionResult GetActiveBookingTypes([FromQuery] string fields)
        {
            
            if (!_typeHelperService.TypeHasProperties<BookingTypeDto>(fields))
            {
                return BadRequest();
            }

            var bookingTypes = _bookingTypeRepository.FindBy(o => o.IsActive).ToList();
            if (!bookingTypes.Any())
            {
                return NotFound();
            }

            var bookingTypeDtos = Mapper.Map<IEnumerable<BookingTypeDto>>(bookingTypes);

            return Ok(new{ bookingTypes = bookingTypeDtos.ShapeData(fields) });
        }
    }
}