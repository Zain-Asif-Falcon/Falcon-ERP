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

namespace ERPSystemAPI.Controllers.V1
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<CompanyController> _logger;
        public CompanyController(ILogger<CompanyController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.Company.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var companyInfo = await _IUnitOfWork.comp.GetAll();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Company.GetActives)]
        public async Task<IActionResult> GetActives()
        {
            var companyInfo = await _IUnitOfWork.comp.GetAll(filter: x => x.IsActive == true);
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Company.GetNonActives)]
        public async Task<IActionResult> GetNonActives()
        {
            var companyInfo = await _IUnitOfWork.comp.GetAll(filter: x => x.IsActive == false);
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Company.Get)]
        public async Task<IActionResult> Get(int companyId)
        {
            if (companyId > 0)
            {
                var companyInfo = await _IUnitOfWork.comp.GetFirstorDefault(companyId);
                return Ok(companyInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Company Code Id" }
                });
            }
        }

        [HttpDelete(ApiRoutes.Company.Delete)]
        public async Task<IActionResult> Delete(int companyId)
        {
            try
            {
                var delItem = await _IUnitOfWork.comp.SetRecordAsDeleted(companyId);
                return Ok(delItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut(ApiRoutes.Company.Update)]
        public async Task<IActionResult> Update([FromBody] Company cmp)
        {
            try
            {
                var updateItem = await _IUnitOfWork.comp.Update(cmp);
                return Ok(updateItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost(ApiRoutes.Company.Create)]
        public async Task<IActionResult> Create([FromBody] Company cmp)
        {
            try
            {
                var addItem = await _IUnitOfWork.comp.Create(cmp);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet(ApiRoutes.Company.ChkExisting)]
        public async Task<IActionResult> GetExisting(string Name)
        {
            var companyInfo = await _IUnitOfWork.comp.GetExisting(Name);
            return Ok(companyInfo);
        }
    }
}
