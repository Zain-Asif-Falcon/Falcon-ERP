using Application;
using ERPSystemAPI.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystemAPI.Controllers.V1
{
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<DashboardController> _logger;
        public DashboardController(ILogger<DashboardController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.Dashboard.GetTicketsCounts)]
        public async Task<IActionResult> GetTicketsCounts()
        {
            var companyInfo = await _IUnitOfWork.dashboard.GetTicketsCounts();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Dashboard.GetFirstFiveRecords)]
        public async Task<IActionResult> GetLatestFiveList()
        {
            var companyInfo = await _IUnitOfWork.dashboard.GetLastFiveList();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Dashboard.GetMonthlyTotRecords)]
        public async Task<IActionResult> GetMonthlyTransactions(DateTime dat)
        {
            var companyInfo = await _IUnitOfWork.dashboard.GetMonthlyTotals(dat);
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Dashboard.GetGraphWeekRecords)]
        public async Task<IActionResult> GetGraphWeekRecordsList()
        {
            var companyInfo = await _IUnitOfWork.dashboard.GetGraphWeeklyList();
            return Ok(companyInfo);
        }
    }
}
