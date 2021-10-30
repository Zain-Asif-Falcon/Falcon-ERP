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
    public class AccountCodeController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<AccountCodeController> _logger;
        public AccountCodeController(ILogger<AccountCodeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.AccountCode.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetAllAccountCodes();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountCode.GetActives)]
        public async Task<IActionResult> GetActives()
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetAllAccountCodes();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountCode.GetNonActives)]
        public async Task<IActionResult> GetNonActives()
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetAll(filter: x => x.IsActive == false);
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountCode.Get)]
        public async Task<IActionResult> Get(int accountcodeId)
        {
            if (accountcodeId > 0)
            {
                var companyInfo = await _IUnitOfWork.AccCode.GetFirstorDefault(accountcodeId);
                return Ok(companyInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Account Code Id" }
                });
            }
        }

        [HttpDelete(ApiRoutes.AccountCode.Delete)]
        public async Task<IActionResult> Delete(int accountcodeId)
        {
            try
            {
                var delItem = await _IUnitOfWork.AccCode.SetRecordAsDeleted(accountcodeId);
                return Ok(delItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut(ApiRoutes.AccountCode.Update)]
        public async Task<IActionResult> Update([FromBody] AccountCode accCod)
        {
            try
            {
                var updateItem = await _IUnitOfWork.AccCode.Update(accCod);
                return Ok(updateItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost(ApiRoutes.AccountCode.Create)]
        public async Task<IActionResult> Create([FromBody] AccountCode accCod)
        {
            try
            {
                var addItem = await _IUnitOfWork.AccCode.SaveCustomAcountCode(accCod);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet(ApiRoutes.AccountCode.Dropdown)]
        public async Task<IActionResult> Dropdown()
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetListAccountCodeForDropDown();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountCode.ChkExisting)]
        public async Task<IActionResult> GetExistingCode(string Code)
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetExistingCode(Code);
            return Ok(companyInfo);
        }
        //==================================================================
        [HttpGet(ApiRoutes.AccountCode.GetAllRepo)]
        public async Task<IActionResult> GetAllRepo()
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetAllAccountCodesRepo();
            return Ok(companyInfo);
        }
    }
}
