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
    public class MonthOpeningController : Controller
    {
        [BindProperty]
        public MonthOpeningsViewModel monthOpenVM { get; set; }
        private readonly IConfiguration _config;
        public MonthOpeningController(IConfiguration config)
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
            monthOpenVM = new MonthOpeningsViewModel();
            ViewBag.AccountHeads = await DropDownData();
            if (id != null)
            {
                var monOpen = await GetById((int)id);
                monthOpenVM.monthopening.MonthOpeningsId = monOpen.MonthOpeningsId;
                monthOpenVM.monthopening.closeStatus = monOpen.closeStatus;
                monthOpenVM.monthopening.ClosingBalance = monOpen.ClosingBalance;
                monthOpenVM.monthopening.DateClosing = monOpen.DateClosing;
                monthOpenVM.monthopening.DateOpening = monOpen.DateOpening;
                monthOpenVM.monthopening.Name = monOpen.Name;
                monthOpenVM.monthopening.OpeningBalance = monOpen.OpeningBalance;
                monthOpenVM.monthopening.IsActive = monOpen.IsActive;
                monthOpenVM.monthopening.Notes = monOpen.Notes;
            }

            return View(monthOpenVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                MonthOpenings montOpen = new MonthOpenings();
                montOpen.MonthOpeningsId = monthOpenVM.monthopening.MonthOpeningsId;
                montOpen.closeStatus = monthOpenVM.monthopening.closeStatus;
                montOpen.ClosingBalance = monthOpenVM.monthopening.ClosingBalance;
                montOpen.DateClosing = monthOpenVM.monthopening.DateClosing;
                montOpen.DateOpening = monthOpenVM.monthopening.DateOpening;
                montOpen.Name = monthOpenVM.monthopening.DateOpening.ToString("MMMM-yy"); //monthOpenVM.monthopening.Name;
                montOpen.OpeningBalance = monthOpenVM.monthopening.OpeningBalance;
                montOpen.DateOpening = monthOpenVM.monthopening.DateOpening;
                montOpen.Notes = monthOpenVM.monthopening.Notes;
                if (monthOpenVM.monthopening.MonthOpeningsId == 0)
                {
                    montOpen.created_at = DateTime.Now;
                    montOpen.IsActive = true;
                    var res = await Create(montOpen);
                    //if(res == false)
                    //{
                        return Json(res);
                    //}
                }
                else
                {
                    montOpen.updated_at = DateTime.Now;
                    montOpen.IsActive = monthOpenVM.monthopening.IsActive;
                    await Update(montOpen);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(monthOpenVM);
            }
        }
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/getall";
            var res = await ApiCalls<MonthOpenings>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/getactives";
            var res = await ApiCalls<MonthOpenings>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/getnonactives";
            var res = await ApiCalls<MonthOpenings>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/{Id}";
            var res = await ApiCalls<MonthOpenings>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<bool> Create(MonthOpenings accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/";
            var res = await ApiCalls<MonthOpenings>.Add(accC, apiUrl);
            return res;
        }
        [HttpPut]
        public async Task<IActionResult> Update(MonthOpenings accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/update";
            var res = await ApiCalls<MonthOpenings>.Update(accH, apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<MonthOpenings> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/{Id}";
            Task<MonthOpenings> res;
            res = ApiCalls<MonthOpenings>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> DropDownData()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/dropdown/";
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
