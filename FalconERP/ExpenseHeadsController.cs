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
    public class ExpenseHeadsController : Controller
    {
        [BindProperty]
        public ExpenseHeadsViewModel expHeadsVM { get; set; }
        private readonly IConfiguration _config;
        public ExpenseHeadsController(IConfiguration config)
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
            expHeadsVM = new ExpenseHeadsViewModel();
            ViewBag.AccountHeads = await DropDownData();
            if (id != null)
            {
                var petties = await GetById((int)id);
                expHeadsVM.expHeads.ExpenseHeadId = petties.ExpenseHeadId;
                expHeadsVM.expHeads.Description = petties.Description;
                expHeadsVM.expHeads.Name = petties.Name;
                expHeadsVM.expHeads.IsActive = petties.IsActive;
            }

            return View(expHeadsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                ExpenseHead exphd = new ExpenseHead();
                exphd.ExpenseHeadId = expHeadsVM.expHeads.ExpenseHeadId;
                exphd.Description = expHeadsVM.expHeads.Description;
                exphd.Name = expHeadsVM.expHeads.Name;
                exphd.IsActive = true;
                exphd.created_at = DateTime.Now;
                if (expHeadsVM.expHeads.ExpenseHeadId == 0)
                {
                    await Create(exphd);
                }
                else
                {
                    await Update(exphd);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(expHeadsVM);
            }
        }
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/getall";
            var res = await ApiCalls<ExpenseHead>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/getactives";
            var res = await ApiCalls<ExpenseHead>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/getnonactives";
            var res = await ApiCalls<ExpenseHead>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/{Id}";
            var res = await ApiCalls<ExpenseHead>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExpenseHead accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/";
            var res = await ApiCalls<ExpenseHead>.Add(accC, apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(ExpenseHead accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/update";
            var res = await ApiCalls<ExpenseHead>.Update(accH, apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<ExpenseHead> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/{Id}";
            Task<ExpenseHead> res;
            res = ApiCalls<ExpenseHead>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> DropDownData()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenseheads/dropdown/";
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
