using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Utilities;
using Domain.Entities;
using Domain.ViewModel.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace ERPSystem.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class PettyCashController : Controller
    {
        [BindProperty]
        public PettyCashViewModel pettyCashVM { get; set; }
        private readonly IConfiguration _config;
        public PettyCashController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            pettyCashVM = new PettyCashViewModel();
            ViewBag.AccountHeads = await DropDownData();
            if (id != null)
            {
                var petties = await GetById((int)id);
                pettyCashVM.petty.PettyCashId = petties.PettyCashId;
                pettyCashVM.petty.MonthOpeningsId = petties.MonthOpeningsId;
                pettyCashVM.petty.Amount = petties.Amount;
                pettyCashVM.petty.Notes = petties.Notes;
                pettyCashVM.petty.date = petties.date;
            }

            return View(pettyCashVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                PettyCash pety = new PettyCash();
                pety.PettyCashId = pettyCashVM.petty.PettyCashId;
                pety.MonthOpeningsId = pettyCashVM.petty.MonthOpeningsId;
                pety.Amount = pettyCashVM.petty.Amount;
                pety.Notes = pettyCashVM.petty.Notes;
                pety.date = pettyCashVM.petty.date;
                if (pettyCashVM.petty.PettyCashId == 0)
                {
                    pety.IsActive = true;
                    pety.created_at = DateTime.Now;
                    await Create(pety);
                }
                else
                {
                    pety.IsActive = pettyCashVM.petty.IsActive;
                    pety.updated_at = DateTime.Now;
                    await Update(pety);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(pettyCashVM);
            }
        }
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/getall";
            var res = await ApiCalls<PettyCashViewModel>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/getactives";
            var res = await ApiCalls<PettyCash>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/getnonactives";
            var res = await ApiCalls<PettyCash>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/{Id}";
            var res = await ApiCalls<PettyCash>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(PettyCash accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/";
            var res = await ApiCalls<PettyCash>.Add(accC, apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(PettyCash accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/update";
            var res = await ApiCalls<PettyCash>.Update(accH, apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<PettyCash> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/{Id}";
            Task<PettyCash> res;
            res = ApiCalls<PettyCash>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> DropDownData()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/pettycash/dropdown/";
                string res = await ApiCalls<string>.GetDropDownList(apiUrl);
                List<SelectListItem> record = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(res);
                return record;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
