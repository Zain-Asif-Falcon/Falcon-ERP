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
    public class PartyCodeController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<ItemsController> _logger;
        public PartyCodeController(ILogger<ItemsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.PartyCode.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var companyInfo = await _IUnitOfWork.partCod.GetAll(orderBy: x => x.OrderByDescending(p => p.PartyCodeId));
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.PartyCode.GetActives)]
        public async Task<IActionResult> GetActives()
        {
            var companyInfo = await _IUnitOfWork.partCod.GetPartyCodesList();//.GetAll(filter: x => x.IsActive == true);
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.PartyCode.GetNonActives)]
        public async Task<IActionResult> GetNonActives()
        {
            var companyInfo = await _IUnitOfWork.partCod.GetAll(filter: x => x.IsActive == false, orderBy: x => x.OrderByDescending(p => p.PartyCodeId));
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.PartyCode.Get)]
        public async Task<IActionResult> Get(int partyCodeId)
        {
            if (partyCodeId > 0)
            {
                var companyInfo = await _IUnitOfWork.partCod.GetFirstorDefault(partyCodeId);
                return Ok(companyInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Item Id" }
                });
            }
        }

        [HttpDelete(ApiRoutes.PartyCode.Delete)]
        public async Task<IActionResult> Delete(int partyCodeId)
        {
            try
            {
                var delItem = await _IUnitOfWork.partCod.SetRecordAsDeleted(partyCodeId);
                return Ok(delItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut(ApiRoutes.PartyCode.Update)]
        public async Task<IActionResult> Update([FromBody] PartyCode pCod)
        {
            try
            {
                var updateItem = await _IUnitOfWork.partCod.Update(pCod);
                return Ok(updateItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost(ApiRoutes.PartyCode.Create)]
        public async Task<IActionResult> Create([FromBody] PartyCode pCod)
        {
            try
            {
                pCod.IsActive = true;
                var addItem = await _IUnitOfWork.partCod.Create(pCod);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet(ApiRoutes.PartyCode.Dropdown)]
        public async Task<IActionResult> Dropdown()
        {
            var companyInfo = await _IUnitOfWork.partCod.GetListPartyCodeForDropDown();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.PartyCode.ChkExisting)]
        public async Task<IActionResult> GetExisting(string Name)
        {
            var companyInfo = await _IUnitOfWork.partCod.GetExisting(Name);
            return Ok(companyInfo);
        }
        //================================================
        [HttpGet(ApiRoutes.PartyCode.GetRepo)]
        public async Task<IActionResult> GetRepo(int accountCodeId)
        {
            var companyInfo = await _IUnitOfWork.partCod.GetPartyCodeRepo(accountCodeId);
            return Ok(companyInfo);
        }
    }
}
