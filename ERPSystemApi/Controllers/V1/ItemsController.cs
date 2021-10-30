using Application;
using Domain.Contracts.V1;
using Domain.Entities;
using Domain.ViewModel.API;
using ERPSystemAPI.ActionFilters;
using ERPSystemAPI.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystemAPI.Controllers
{
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<ItemsController> _logger;
        public ItemsController(ILogger<ItemsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.Items.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var companyInfo = await _IUnitOfWork.Itm.GetAll(orderBy: x => x.OrderByDescending(p => p.ItemsId));
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Items.GetActives)]
        public async Task<IActionResult> GetActives()
        {
            var companyInfo = await _IUnitOfWork.Itm.GetAll(filter: x => x.IsActive == true,orderBy:x => x.OrderByDescending(p => p.ItemsId));
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Items.GetNonActives)]
        public async Task<IActionResult> GetNonActives()
        {
            var companyInfo = await _IUnitOfWork.Itm.GetAll(filter: x => x.IsActive == false, orderBy: x => x.OrderByDescending(p => p.ItemsId));
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Items.Get)]
        public async Task<IActionResult> Get(int itemId)
        {
            if (itemId > 0)
            {
                var companyInfo = await _IUnitOfWork.Itm.GetItemInfo(itemId);
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

        [HttpDelete(ApiRoutes.Items.Delete)]
        public async Task<IActionResult> Delete(int itemId)
        {
            try
            {
                var delItem = await _IUnitOfWork.Itm.SetRecordAsDeleted(itemId);
                return Ok(delItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut(ApiRoutes.Items.Update)]
        public async Task<IActionResult> Update([FromBody] Item itms)
        {
            try
            {
                var updateItem = await _IUnitOfWork.Itm.Update(itms);
                return Ok(updateItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost(ApiRoutes.Items.Create)]
        public async Task<IActionResult> Create([FromBody] Item itms)
        {
            try
            {
                var addItem = await _IUnitOfWork.Itm.Create(itms);
                return Ok(addItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet(ApiRoutes.Items.Dropdown)]
        public async Task<IActionResult> Dropdown()
        {
            var companyInfo = await _IUnitOfWork.Itm.GetListItemsForDropDown();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Items.Counts)]
        public async Task<IActionResult> GetCounts()
        {
            var companyInfo = _IUnitOfWork.Itm.GetAllRecordsCount();
            return Ok(companyInfo);
        }
        [HttpGet(ApiRoutes.Items.ChkExisting)]
        public async Task<IActionResult> GetExistingCode(string Code)
        {
            var companyInfo = await _IUnitOfWork.Itm.GetExixtingCode(Code);
            return Ok(companyInfo);
        }
    }
}
