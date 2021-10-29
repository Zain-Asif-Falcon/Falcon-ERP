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

namespace ERPSystem.Areas.Accounts.Controllers
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
        public async Task<IActionResult> Upsert(int? id)
        {
            dayOpenVM = new DayOpeningsViewModel();
            if (id != null)
            {
                var dayOpen = await GetById((int)id);
                dayOpenVM.dayopening.DayOpeningsId = dayOpen.DayOpeningsId;
                dayOpenVM.dayopening.DateTimeOpening = dayOpen.DateTimeOpening;
                dayOpenVM.dayopening.OpeningBalance = dayOpen.OpeningBalance;
                dayOpenVM.dayopening.MonthOpeningsId = dayOpen.MonthOpeningsId;
                dayOpenVM.dayopening.closeStatus = dayOpen.closeStatus;
                dayOpenVM.dayopening.ClosingBalance = dayOpen.ClosingBalance;
                dayOpenVM.dayopening.DateTimeClosing = dayOpen.DateTimeClosing;
                dayOpenVM.dayopening.BankDeposit = dayOpen.BankDeposit;
                dayOpenVM.dayopening.cashDifference = dayOpen.cashDifference;
                dayOpenVM.dayopening.cashInHand = dayOpen.cashInHand;
                if(dayOpen.sales == null)
                {
                    var sumBal = await GetAllSums((int)id);

                    dayOpenVM.dayopening.sales = sumBal.sales;
                    dayOpenVM.dayopening.Purchases = sumBal.purchases;
                    dayOpenVM.dayopening.cashPayments = sumBal.payments;
                    dayOpenVM.dayopening.cashReciepts = sumBal.reciepts;
                    dayOpenVM.dayopening.expenses = sumBal.expenses;
                    dayOpenVM.dayopening.totalCash = (sumBal.sales + sumBal.reciepts) - (sumBal.purchases + sumBal.payments + sumBal.expenses);
                }
                else
                {
                    dayOpenVM.dayopening.sales = dayOpen.sales;
                    dayOpenVM.dayopening.Purchases = dayOpen.Purchases;
                    dayOpenVM.dayopening.cashPayments = dayOpen.cashPayments;
                    dayOpenVM.dayopening.cashReciepts = dayOpen.cashReciepts;
                    dayOpenVM.dayopening.expenses = dayOpen.expenses;
                    dayOpenVM.dayopening.totalCash = dayOpen.totalCash;
                }

                dayOpenVM.dayopening.Notes = dayOpen.Notes;
            }
            else
            {
                var openingBal = await GetNewOpeningBalance();
                dayOpenVM.dayopening.OpeningBalance = openingBal.openingBalance;
            }

            return View(dayOpenVM);
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
        [HttpPost]
        public async Task<bool> Create(DayOpenings accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/dayopenings/";
            var res = await ApiCalls<DayOpenings>.Add(accC, apiUrl);
            return res;
        }
        [HttpPut]
        public async Task<IActionResult> Update(DayOpenings accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/dayopenings/update";
            var res = await ApiCalls<DayOpenings>.Update(accH, apiUrl);
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
