using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.RoomRent.Controllers;

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
        [HttpPost("book", Name = "book")]
        public IActionResult Book(GuestBookingDetail model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(HandleApiException("Error when booking.", HttpStatusCode.NotFound));
                }
                else
                {
                    //save both parent and children guests details
                    _guestBookingDetailRepository.Add(model);
                    _guestBookingDetailRepository.Commit();

                    return Ok(new { success = true, message = "Booking has been successful" });
                }

            }
            catch (Exception ex)
            {
                return Ok(HandleApiException(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }

        private BaseRestApiModel HandleApiException(string message, HttpStatusCode httpStatusCode)
        {
            return new BaseRestApiModel
            {
                Payload = new List<object>(),
                Errors = new BaseErrorModel
                {
                    Message = message,
                    MessageFields = ""
                },
                StatusCode = (int)httpStatusCode
            };
        }
    }
}