using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.Infrastructure.Helpers;
using RicMonitoringAPI.RicXplorer.ViewModels;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/guest-booking")]
    [ApiController]
    public class GuestBookingController : ControllerBase
    {
        private readonly IGuestBookingDetailRepository _guestBookingDetailRepository;

        public GuestBookingController(IGuestBookingDetailRepository guestBookingDetailRepository)
        {
            _guestBookingDetailRepository = guestBookingDetailRepository ?? throw new ArgumentNullException(nameof(guestBookingDetailRepository));
        }

        [AllowAnonymous]
        [HttpGet("booked-guests")]
        public IActionResult BookedGuests(string startDate, string endDate)
        {
            DateTime.TryParse(startDate, out DateTime arrivalDate);
            DateTime.TryParse(endDate, out DateTime departureDate);

            var guests = _guestBookingDetailRepository.FindBookings(arrivalDate, departureDate);

            return Ok(new BaseRestApiModel
            {
                Payload = guests,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [AllowAnonymous]
        [HttpPost("book", Name = "book")]
        public IActionResult Book(GuestBookingDetailDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(HandleApi.Exception("Error when booking.", HttpStatusCode.NotFound));
                }
                else
                {
                    //save both parent and children guests details
                    var guestBookingDetail = Mapper.Map<GuestBookingDetail>(model);

                    _guestBookingDetailRepository.Add(guestBookingDetail);
                    _guestBookingDetailRepository.Commit();

                    return Ok(new BaseRestApiModel
                    {
                        Payload = "Booking has been successful",
                        Errors = new List<BaseError>(),
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }
                
            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }

    }
}