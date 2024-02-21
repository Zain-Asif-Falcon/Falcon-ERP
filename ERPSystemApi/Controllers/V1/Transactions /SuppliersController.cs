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

namespace ERPSystemAPI.Controllers.V1.Transactions
{
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<SuppliersController> _logger;
        public SuppliersController(ILogger<SuppliersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
   
        [HttpGet(ApiRoutes.Supplier.GetActives)]
        public async Task<IActionResult> GetActives()
        {
            var companyInfo = await _IUnitOfWork.supp.GetAll(filter: x => x.IsActive == true);
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Supplier.GetNonActives)]
        public async Task<IActionResult> GetNonActives()
        {
            var companyInfo = await _IUnitOfWork.supp.GetAll(filter: x => x.IsActive == false);
            return Ok(companyInfo);
        }
       
        [HttpPost(ApiRoutes.Supplier.Create)]
        public async Task<IActionResult> Create([FromBody] Supplier accCod)
        {
            try
            {
                var addItem = await _IUnitOfWork.supp.Create(accCod);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet(ApiRoutes.Supplier.Dropdown)]
        public async Task<IActionResult> Dropdown()
        {
            var companyInfo = await _IUnitOfWork.supp.GetListForDropDown();
            return Ok(companyInfo);
        }
    }
}
