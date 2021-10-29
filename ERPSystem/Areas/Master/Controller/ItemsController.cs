using Application.Common.Utilities;
using Application.Services.IServiceCaller;
using Domain.Contracts.V1;
using Domain.Contracts.Wrappers;
using Domain.Entities;
using Domain.ViewModel;
using Jose;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Areas.Items.Controllers
{
    [Area("Master")]
    //[Route("Master/[controller]/[action]")]
    public class ItemsController : Controller
    {
        [BindProperty]
        public ItemsViewModel ItemsVM { get; set; }
        private readonly IConfiguration _config;
        public ItemsController(IConfiguration config) 
        {
            //_serviceCaller = _service;
            _config = config;
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {

            ItemsVM = new ItemsViewModel();

            if (id != null)
            {
                var itms = await GetById((int)id);
                ItemsVM.Itms.ItemsId = itms.Itms.ItemsId;
                ItemsVM.Itms.Code = itms.Itms.Code;
                ItemsVM.Itms.Description = itms.Itms.Description;
                ItemsVM.Itms.Price = itms.Itms.Price;
                ItemsVM.Itms.PurchasePrice = itms.Itms.PurchasePrice;
                ItemsVM.Itms.Weight = itms.Itms.Weight;
                //familyOfActionVM = _unitOfWork.DiagFamily.Get(id.GetValueOrDefault());
            }

            return View(ItemsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                Item itm = new Item();
                itm.ItemsId = ItemsVM.Itms.ItemsId;
                itm.Code = ItemsVM.Itms.Code;
                itm.Description = ItemsVM.Itms.Description;
                itm.Price = ItemsVM.Itms.Price;
                itm.PurchasePrice = ItemsVM.Itms.PurchasePrice;
                itm.Weight = ItemsVM.Itms.Weight;
                if (ItemsVM.Itms.ItemsId == 0)
                {
                    itm.IsActive = true;
                    itm.created_at = DateTime.Now;
                    await Create(itm);
                }
                else
                {
                    itm.updated_at = DateTime.Now;
                    await Update(itm);
                }
                //_unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(ItemsVM);
            }
        }
        #region API Calling
      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/getall";
            var res = await ApiCalls<Item>.GetData(apiUrl); 
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/getactives";
            var res = await ApiCalls<Item>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/getnonactives";
            var res = await ApiCalls<Item>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/{Id}";
            var res = await ApiCalls<Item>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Item itm)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/";
            var res = await ApiCalls<Item>.Add(itm,apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(Item itm)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/update";
            var res = await ApiCalls<Item>.Update(itm, apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<ItemsViewModel> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/{Id}";
            Task<ItemsViewModel> res;
            res = ApiCalls<ItemsViewModel>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IActionResult> CheckExistingCode(string Code)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/CheckCode/{Code}";
            var res = await ApiCalls<Item>.CheckExisting(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        #endregion
    }
}
