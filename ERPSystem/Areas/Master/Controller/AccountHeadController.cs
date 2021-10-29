using Application.Common.Utilities;
using Domain.Contracts.V1;
using Domain.Entities;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Areas.Master.Controllers
{
    [Area("Master")]
    public class AccountHeadController : Controller
    {
        [BindProperty]
        public AccountHeadsViewModel AccHeadVM { get; set; }
        private readonly IConfiguration _config;
        public AccountHeadController(IConfiguration config)
        {
            _config = config;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            AccHeadVM = new AccountHeadsViewModel();

            if (id != null)
            {
                var accHeads = await GetById((int)id);
                AccHeadVM.AccHead.AccountHeadId = accHeads.AccountHeadId;
                AccHeadVM.AccHead.Code = accHeads.Code;
                AccHeadVM.AccHead.Description = accHeads.Description;
            }

            return View(AccHeadVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                AccountHead accH = new AccountHead();
                accH.AccountHeadId = AccHeadVM.AccHead.AccountHeadId;
                accH.Code = AccHeadVM.AccHead.Code;
                accH.Description = AccHeadVM.AccHead.Description;
                if (AccHeadVM.AccHead.AccountHeadId == 0)
                {
                    accH.IsActive = true;
                    accH.created_at = DateTime.Now;
                    await Create(accH);
                }
                else
                {
                    accH.updated_at = DateTime.Now;
                    await Update(accH);
                }
                //_unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(AccHeadVM);
            }
        }
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/getall";
            var res = await ApiCalls<AccountHead>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/getactives";
            var res = await ApiCalls<AccountHead>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/getnonactives";
            var res = await ApiCalls<AccountHead>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/{Id}";
            var res = await ApiCalls<AccountHead>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(AccountHead accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/";
            var res = await ApiCalls<AccountHead>.Add(accH, apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(AccountHead accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/update";
            var res = await ApiCalls<AccountHead>.Update(accH, apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<AccountHead> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/{Id}";
            Task<AccountHead> res;
            res = ApiCalls<AccountHead>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IActionResult> CheckExistingCode(string Code)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/CheckCode/{Code}";
            var res = await ApiCalls<AccountHead>.CheckExisting(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        #endregion
    }
}
