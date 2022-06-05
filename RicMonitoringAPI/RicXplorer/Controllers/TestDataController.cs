using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;
using RicMonitoringAPI.Common.Enumeration;
using RicMonitoringAPI.Infrastructure.Helpers;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [AllowAnonymous]
    [Route("api/test-data")]
    [ApiController]
    public class TestDataController : ControllerBase
    {
        private readonly IGuestBookingDetailRepository _guestBookingDetailRepository;


        public TestDataController(
            IGuestBookingDetailRepository guestBookingDetailRepository)
        {
            _guestBookingDetailRepository = guestBookingDetailRepository ?? throw new ArgumentNullException(nameof(guestBookingDetailRepository));
        }



        [HttpPost(Name = "PostBooking")]
        public async Task<IActionResult> PostBooking()
        {
            var startDate = new DateTime(2022, 07, 10);

            //CreateGuest(startDate, startDate.AddDays(3), "Glaiza");
            //CreateGuest(startDate, startDate.AddDays(3), "Homer");
            //CreateGuest(startDate, startDate.AddDays(3), "David");

            CreateGuest(startDate, startDate.AddDays(5), "Sherine", 2, BookingTypeEnum.CoupleBackpackers);
            CreateGuest(startDate, startDate.AddDays(5), "Charmaine", 2, BookingTypeEnum.CoupleBackpackers);
            CreateGuest(startDate, startDate.AddDays(5), "Aia", 2, BookingTypeEnum.CoupleBackpackers);

            return Ok("added guests");
        }

        private void CreateGuest(DateTime arrive, DateTime depart, string firstName = "Eric", int noOfGuests = 1, BookingTypeEnum bookingType = BookingTypeEnum.Backpacker)
        {
            var guests = new List<GuestBooking>();
            for (int i = 0; i < noOfGuests; i++)
            {

                var guest = new GuestBooking
                {
                    FirstName = $"{firstName} - {i+1}",
                    LastName = "Ramos",
                    Birthday = new DateTime(1986, 1, 1),
                    Gender = "Male",
                    Ages = 1
                };
                guest.Age = Calculate.Age(guest.Birthday);

                guests.Add(guest);

            }
            
            var model = new GuestBookingDetail
            {
                AccountId = 1, //agatha TODO: change to 999
                ArrivalDate = arrive,
                DepartureDate = depart,
                Contact = 091111111.ToString(),
                Country = "Philippines",
                CreatedDateTimeUtc = DateTime.UtcNow,
                Email = "ph@yahoo.com",
                LanguagesSpoken = "English",
                LeaveMessage = "Testing Purposes",
                BookingType = (int) bookingType,
                GuestBookings = guests
            };

            AddGuest(model);
        }

        public void AddGuest(GuestBookingDetail model)
        {
            model.GuestBookingDates = new List<GuestBookingDate>();
            for (DateTime startDate = model.ArrivalDate; startDate <= model.DepartureDate; startDate = startDate.AddDays(1))
            {
                model.GuestBookingDates.Add(new GuestBookingDate
                {
                    DateBooked = startDate
                });
            }

            _guestBookingDetailRepository.Add(model);
            _guestBookingDetailRepository.Commit();
        }
    }
}