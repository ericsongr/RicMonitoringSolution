using AutoMapper;
using Bogus;
using Bogus.DataSets;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RicEntityFramework;
using RicEntityFramework.RicXplorer.Interfaces;
using RicEntityFramework.RicXplorer.Repositories;
using RicModel.RicXplorer;
using RicMonitoringAPI.RicXplorer.Controllers;
using RicMonitoringAPI.RicXplorer.ViewModels;

namespace RicUnitTest
{
    public  class GuestBookingControllerUnitTest
    {
        private readonly Mock<IGuestBookingDetailRepository> _guestBookingDetailRepository;
        private readonly Mock<IGuestCheckListRepository> _guestCheckListRepository;
        private readonly Mock<ILookupTypeRepository> _lookupTypeRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly GuestBookingController _controller;
        public GuestBookingControllerUnitTest()
        {
            //var db = new RicDbContext();
            //_guestBookingDetailRepository = new GuestBookingDetailRepository();
            //_guestCheckListRepository = guestCheckListRepository;
            //_lookupTypeRepository = lookupTypeRepository;
            //_mapper = mapper;

            _guestBookingDetailRepository = new Mock<IGuestBookingDetailRepository>();
            _guestCheckListRepository = new Mock<IGuestCheckListRepository>();
            _lookupTypeRepository = new Mock<ILookupTypeRepository>();
            _mapper = new Mock<IMapper>();

            _controller = new GuestBookingController(
                _guestBookingDetailRepository.Object,
                _guestCheckListRepository.Object,
                _lookupTypeRepository.Object,
                _mapper.Object);

        }

        [Fact]
        public void GuestBookingController_Book_Return_Int()
        {

            //Arrange
            var faker = new Faker();
            var arrivalDate = new DateTime(2024, 7, 11);
            var departureDate = new DateTime(2024, 7, 13);
            var guestBookings = new List<GuestBookingDto>
            {
                new GuestBookingDto
                {
                    Birthday = new DateTime(1980, 9, 19),
                    FirstName = faker.Name.FirstName(Name.Gender.Male),
                    LastName = faker.Name.LastName(Name.Gender.Male),
                    Gender = "Male",
                    Ages = 1
                }
            };

            var guestBooking = new CreateGuestBookingDto
            {
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                BookingType = 1,
                Contact = "+069999999999",
                ContactPerson = $"{guestBookings[0].FirstName} {guestBookings[0].LastName}",
                Country = "Philippines",
                LanguagesSpoken = "English",
                Email = "english@yahoo.com",
                LeaveMessage = "Test leave message",
                SelectedPayment = "GCASH",
                GuestBookings = guestBookings
            };

            var guestBooking2 = new GuestBookingDetail
            {
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                BookingType = 1,
                Contact = "+069999999999",
                ContactPerson = $"{guestBookings[0].FirstName} {guestBookings[0].LastName}",
                Country = "Philippines",
                LanguagesSpoken = "English",
                Email = "english@yahoo.com",
                LeaveMessage = "Test leave message",
                GuestBookings = new List<GuestBooking>
                {
                    new GuestBooking
                    {
                        Birthday = new DateTime(1980, 9, 19),
                        FirstName = faker.Name.FirstName(Name.Gender.Male),
                        LastName = faker.Name.LastName(Name.Gender.Male),
                        Gender = "Male",
                        Ages = 1
                    }
                }
            };

            _mapper.Setup(mapper => mapper.Map<GuestBookingDetail>(guestBooking)).Returns(guestBooking2);

            //Act
            var result = _controller.Book(guestBooking);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));



        }
    }
}
