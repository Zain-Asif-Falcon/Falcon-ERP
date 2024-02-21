using Application;
using Domain.Entities;
using Domain.ViewModel.API;
using ERPSystemAPI.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystemAPI.Controllers.V1.Accounting
{
    [ApiController]
    public class MonthOpeningsController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<MonthOpeningsController> _logger;
        public MonthOpeningsController(ILogger<MonthOpeningsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.MonthOpenings.Get)]
        public async Task<IActionResult> Get(int monthopenId)
        {
            if (monthopenId > 0)
            {
                var companyInfo = await _IUnitOfWork.monthOpen.GetFirstorDefault(monthopenId);
                return Ok(companyInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Month Opening Code Id" }
                });
            }
        }

        [HttpPost(ApiRoutes.MonthOpenings.Create)]
        public async Task<IActionResult> Create([FromBody] MonthOpenings monthOpen)
        {
            try
            {
                var addItem = await _IUnitOfWork.monthOpen.CreateCustom(monthOpen);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
