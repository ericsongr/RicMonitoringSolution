using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RicMonitoringAPI.Common.Helpers;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.Common.Controllers
{
    [Route("api/app-functions")]
    [ApiController]
    public class AppFunctionsController : ControllerBase
    {
        [HttpGet(Name = "days-with-suffix")]
        public IActionResult DaysWithSuffix(string selectedDate)
        {

            var isValid = DateTime.TryParse(selectedDate, out DateTime dateStarted);
            if (!isValid)
                return BadRequest("Invalid date format");

            var lastDayOfTheMonth = DateTime.DaysInMonth(dateStarted.Year, dateStarted.Month);

            var daysOptions = new List<AppSelectListItem>();
            for (int i = 1; i <= lastDayOfTheMonth; i++)
            {
                daysOptions.Add(new AppSelectListItem
                {
                    Value = i.ToString(),
                    Text = $"{i.ToString()}{CommonFunctions.GetSuffix(i.ToString())}"
                });
            }

            return Ok(daysOptions);
        }
    }
}