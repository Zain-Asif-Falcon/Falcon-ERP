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
    public class ExpensesController : Controller
    {
        [BindProperty]
        public ExpensesViewModel expVM { get; set; }
        private readonly IConfiguration _config;
        public ExpensesController(IConfiguration config)
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
            expVM = new ExpensesViewModel();
            ViewBag.ExpenseHeads = await DropDownData();
            if (id != null)
            {
                var exps = await GetById((int)id);
                expVM.expenses.Name = exps.Name;
                expVM.expenses.ExpensesId = exps.ExpensesId;
                expVM.expenses.MonthOpeningsId = exps.MonthOpeningsId;
                expVM.expenses.Amount = exps.Amount;
                expVM.expenses.Notes = exps.Notes;
                expVM.expenses.date = exps.date;
                expVM.expenses.ExpenseHeadsId = exps.ExpenseHeadsId;
            }

            return View(expVM);
        }
     
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenses/getall";
            var res = await ApiCalls<ExpensesViewModel>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }    

        [HttpPost]
        public async Task<IActionResult> Create(Expenses accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/expenses/";
            var res = await ApiCalls<Expenses>.Add(accC, apiUrl);
            return Json(new
            {
                data = res
            });
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
