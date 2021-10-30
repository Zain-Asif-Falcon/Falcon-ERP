using Application;
using Domain.Contracts.V1;
using Domain.Entities;
using Domain.ViewModel.API;
using ERPSystemAPI.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystemAPI.Controllers.V1
{
    [ApiController]
    public class AccountHeadsController : Controller
    {
        private readonly IUnitOfWork _IUnitOfWork;

        public AccountHeadsController(IUnitOfWork unitOfWork)
        {
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.AccountHeads.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var companyInfo = await _IUnitOfWork.AccHead.GetAll(orderBy: x => x.OrderByDescending(p => p.AccountHeadId));
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountHeads.Get)]
        public async Task<IActionResult> Get(int accountheadsId)
        {
            if (accountheadsId > 0)
            {
                var companyInfo = await _IUnitOfWork.AccHead.GetFirstorDefault(accountheadsId);
                return Ok(companyInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Account Head Id" }
                });
            }
        }
        [HttpGet(ApiRoutes.AccountHeads.GetActives)]
        public async Task<IActionResult> GetActives()
        {
            var companyInfo = await _IUnitOfWork.AccHead.GetAll(filter: x => x.IsActive == true, orderBy: x => x.OrderByDescending(p => p.AccountHeadId));
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.AccountHeads.GetNonActives)]
        public async Task<IActionResult> GetNonActives()
        {
            var companyInfo = await _IUnitOfWork.AccHead.GetAll(filter: x => x.IsActive == false, orderBy: x => x.OrderByDescending(p => p.AccountHeadId));
            return Ok(companyInfo);
        }
        [HttpPost(ApiRoutes.AccountHeads.Create)]
        public async Task<IActionResult> Create([FromBody] AccountHead acc)
        {
            try
            {
                var addAccHead = await _IUnitOfWork.AccHead.Create(acc);
                return Ok(addAccHead);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
       
        [HttpPut(ApiRoutes.AccountHeads.Update)]
        public async Task<IActionResult> Update([FromBody] AccountHead acc)
        {
            try
            {
                var addAccHead = await _IUnitOfWork.AccHead.Update(acc);
                return Ok(addAccHead);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpDelete(ApiRoutes.AccountHeads.Delete)]
        public async Task<IActionResult> Delete(int accountheadsId)
        {
            try
            {
                var addAccHead = await _IUnitOfWork.AccHead.SetRecordAsDeleted(accountheadsId);
                return Ok(addAccHead);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet(ApiRoutes.AccountHeads.Dropdown)]
        public async Task<IActionResult> Dropdown()
        {
            var companyInfo = await _IUnitOfWork.AccHead.GetListAccountHeadForDropDown();
            return Ok(companyInfo); 
        }
        [HttpGet(ApiRoutes.AccountHeads.ChkExisting)]
        public async Task<IActionResult> GetExistingCode(string Code)
        {
            var companyInfo = await _IUnitOfWork.AccHead.GetExistingCode(Code);
            return Ok(companyInfo);
        }
    }
}