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
    public class ExpensesController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<ExpensesController> _logger;
        public ExpensesController(ILogger<ExpensesController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.Expenses.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var companyInfo = await _IUnitOfWork.expnse.GetAllExpenses();
            return Ok(companyInfo);
        }      
        
        [HttpPost(ApiRoutes.Expenses.Create)]
        public async Task<IActionResult> Create([FromBody] Expenses exphead)
        {
            try
            {
                var addItem = await _IUnitOfWork.expnse.SaveExpense(exphead);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
 
    }
}
