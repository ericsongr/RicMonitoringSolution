using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [Route("api/TestSpRentTransactionBatchFile")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly RicDbContext _context;

        public TestController(
            RicDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// only for test purposes running transaction for 4 months before the page for transaction history created.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TestSpRentTransactionBatchFile()
        {
            return null;
            //var startDate = new DateTime(2020, 1, 1);// january 1, 2020
            //var endDate = startDate.AddMonths(3).AddDays(26); //loop at least 4 months
            //var builder = new StringBuilder();
            //Boolean success = true;

            //try
            //{
            //    while (startDate < endDate)
            //    {
            //        List<SqlParameter> spParams = new List<SqlParameter>
            //        {
            //            new SqlParameter("@CurrentDate", startDate)
            //        };

            //        //execute sp proc here
            //        await _context.Database.ExecuteSqlCommandAsync($"RentTransactionBatchFile @CurrentDate", spParams.ToArray());

            //        builder.Append($"{startDate.ToShortDateString()},");

            //        //add 1 day for every loop until reach the end date
            //        startDate = startDate.AddDays(1);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    success = false;
            //}


            //return Ok(new
            //{
            //    Description = "Sp Run",
            //    Status = success,
            //    ExecutedDates = builder.ToString()
            //});
        }
    }
}