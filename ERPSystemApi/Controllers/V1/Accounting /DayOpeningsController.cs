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
    public class DayOpeningsController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<DayOpeningsController> _logger;
        public DayOpeningsController(ILogger<DayOpeningsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
    
        [HttpGet(ApiRoutes.DayOpenings.Get)]
        public async Task<IActionResult> Get(int dayOpenId)
        {
            if (dayOpenId > 0)
            {
                var companyInfo = await _IUnitOfWork.dayOpen.GetFirstorDefault(dayOpenId);
                return Ok(companyInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Day Opening Code Id" }
                });
            }
        }
 
        [HttpPost(ApiRoutes.DayOpenings.Create)]
        public async Task<IActionResult> Create([FromBody] DayOpenings dayOpen)
        {
            try
            {
                var addItem = await _IUnitOfWork.dayOpen.CreateCustom(dayOpen);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
