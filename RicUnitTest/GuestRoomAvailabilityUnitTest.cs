using System.Net.NetworkInformation;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RicEntityFramework.Services;
using RicMonitoringAPI.RicXplorer.Controllers;

namespace RicUnitTest
{
    public class GuestRoomAvailabilityUnitTest : IClassFixture<WebApplicationFactory<RicMonitoringAPI.Startup>>
    {
        private readonly HttpClient _client;

        public GuestRoomAvailabilityUnitTest(WebApplicationFactory<RicMonitoringAPI.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsWeatherForecasts()
        {
            // Act
            var response = await _client.GetAsync("/WeatherForecast");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("date", content); // Basic check to see if the response contains the date
        }

        [Fact]
        public async Task Post_ReturnsPostedWeatherForecast()
        {
            // Arrange
            var forecast = new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = 25,
                Summary = "Warm"
            };
            var json = JsonConvert.SerializeObject(forecast);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/WeatherForecast", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedForecast = JsonConvert.DeserializeObject<WeatherForecast>(responseString);
            //Assert.Equal(forecast.Date, returnedForecast.Date);
            //Assert.Equal(forecast.TemperatureC, returnedForecast.TemperatureC);
            //Assert.Equal(forecast.Summary, returnedForecast.Summary);

            forecast.Date.Should().Be(returnedForecast.Date);
            forecast.TemperatureC.Should().Be(returnedForecast.TemperatureC);
            forecast.Summary.Should().Be(returnedForecast.Summary);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        [InlineData(3, 3, 6)]
        [InlineData(4, 4, 8)]
        public void TestService_Compute_ReturnInt(int a, int b, int expected)
        {
            //Arrange
            var testService = new TestService();

            //Act
            var result = testService.Compute(a, b);

            //Assert
            result.Should().Be(expected);
            result.Should().BeGreaterOrEqualTo(2);
            result.Should().NotBeInRange(-10000, 0);


        }

        [Fact]
        public void TestService_GetPingOptions_ReturnObject()
        {
            //arrange
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            var pingService = new TestService();
            //act
            var result = pingService.GetPingOptions();

            //Assert WARNING: "Be" careful
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void TestService_MostRecentPings_ReturnObject()
        {
            //Arrange
            var pingService = new TestService();
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };
            //Act
            var result = pingService.MostRecentPings();
            
            //Assert WARNING: "Be" careful
            //result.Should().BeOfType<IEnumerable<PingOptions>>();
            result.Should().ContainEquivalentOf(expected);
            result.Should().Contain(x => x.DontFragment == true);
        }
    }
}