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

       
        [HttpGet(ApiRoutes.AccountCode.Dropdown)]
        public async Task<IActionResult> DropdownAccountCode()
        {
            var companyInfo = await _IUnitOfWork.AccCode.AccountCodeDropDown();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountCode.DropdownName)]
        public async Task<IActionResult> DropdownName()
        {
            var companyInfo = await _IUnitOfWork.AccCode.AccountCodeDropDownByName();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountCode.ChkExisting)]
        public async Task<IActionResult> GetExistingCode(int Code)
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetExistingCode(Code);
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountCode.GetLastAccountCode)]
        public async Task<IActionResult> GetLastAccountCode(int headId)
        {
            var companyInfo = await _IUnitOfWork.AccCode.GetLastAccountCode(headId);
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
