using Application.Common.Utilities;
using Domain.Entities;
using Domain.ViewModel.Accounts;
using Domain.ViewModel.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystem.Areas.Accounts.Controllers //
{
    [Area("Accounts")]
    public class DayOpeningsController : Controller
    {
        [BindProperty]
        public DayOpeningsViewModel dayOpenVM { get; set; }
        private readonly IConfiguration _config;
        public DayOpeningsController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
               [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                DayOpenings dayOpen = new DayOpenings();
                
                if (dayOpenVM.dayopening.DayOpeningsId == 0)
                {
                    dayOpen.DateTimeOpening = dayOpenVM.dayopening.DateTimeOpening;
                    dayOpen.OpeningBalance = dayOpenVM.dayopening.OpeningBalance;
                    var res = await Create(dayOpen);
                    return Json(res);
                }
                else
                {
                    dayOpen.DayOpeningsId = dayOpenVM.dayopening.DayOpeningsId;
                    dayOpen.DateTimeClosing = dayOpenVM.dayopening.DateTimeClosing;
                    dayOpen.BankDeposit = dayOpenVM.dayopening.BankDeposit;
                    dayOpen.cashDifference = dayOpenVM.dayopening.cashDifference;
                    dayOpen.cashInHand = dayOpenVM.dayopening.cashInHand;
                    dayOpen.cashPayments = dayOpenVM.dayopening.cashPayments;
                    dayOpen.cashReciepts = dayOpenVM.dayopening.cashReciepts;
                    dayOpen.expenses = dayOpenVM.dayopening.expenses;
                    dayOpen.Notes = dayOpenVM.dayopening.Notes;
                    dayOpen.Purchases = dayOpenVM.dayopening.Purchases;
                    dayOpen.sales = dayOpenVM.dayopening.sales;
                    dayOpen.totalCash = dayOpenVM.dayopening.totalCash;
                    dayOpen.closeStatus = dayOpenVM.dayopening.closeStatus;
                    dayOpen.ClosingBalance = dayOpenVM.dayopening.ClosingBalance;

                    await Update(dayOpen);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(dayOpenVM);
            }
        }
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/dayopenings/getactives";
            var res = await ApiCalls<DayOpenings>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
       
        [HttpGet]
        public async Task<DayOpenings> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/dayopenings/{Id}";
            Task<DayOpenings> res;
            res = ApiCalls<DayOpenings>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<DayOpeningsSumVM> GetAllSums(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/sumofalldayopening/{Id}";
            Task<DayOpeningsSumVM> res;
            res = ApiCalls<DayOpeningsSumVM>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<NewOpeningBalanceVM> GetNewOpeningBalance()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/dayopenings/openingbalance";
            Task<NewOpeningBalanceVM> res;
            res = ApiCalls<NewOpeningBalanceVM>.GetSingleData(apiUrl);
            return await res;
        }
        #endregion
    }
}
