using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework;
using RicEntityFramework.RoomRent.Interfaces;
using RicMonitoringAPI.Common.Constants;

namespace RicMonitoringAPI.RoomRent.Controllers
{

    [Authorize(Policy = "ProcessTenantsTransaction")]
    [Route("api/exec-store-proc")]
    [ApiController]
    public class ExecuteSqlProcedureController : ControllerBase
    {
        private readonly RicDbContext _context;
        private readonly IMonthlyRentBatchRepository _monthlyRentBatchRepository;

        public ExecuteSqlProcedureController(
            RicDbContext context,
            IMonthlyRentBatchRepository monthlyRentBatchRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _monthlyRentBatchRepository = monthlyRentBatchRepository ?? throw new ArgumentNullException(nameof(monthlyRentBatchRepository));
        }

        [HttpPost()]
        public async Task<IActionResult> ExecRentTransactionBatchFile()
        {
            var currentDate = DateTime.Now;
            var status = DailyBatchStatusConstant.Processing;

            var dailyBatchStatus = _monthlyRentBatchRepository.FindBy(o => o.ProcessStartDateTime.Date == currentDate.Date).ToList();
            if (dailyBatchStatus.Any())
            {
                var item = dailyBatchStatus.FirstOrDefault();
                if (item.ProcesssEndDateTime != null)
                {
                    status = DailyBatchStatusConstant.Processed;
                }
            }
            else
            {
               List<SqlParameter> pc = new List<SqlParameter>()
               {
                   new SqlParameter("@CurrentDate", DateTime.Now)
               };
               
               await _context.Database.ExecuteSqlCommandAsync($"RentTransactionBatchFile @CurrentDate", pc.ToArray());
            }

            //Thread.Sleep(5000);

            return Ok(new {status});
        }

      
    }
}
