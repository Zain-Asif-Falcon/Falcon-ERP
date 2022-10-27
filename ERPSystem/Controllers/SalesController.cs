using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Utilities;
using Domain.Entities;
using Domain.ViewModel.Options;
using Domain.ViewModel.Reports;
using Domain.ViewModel.Transactions;
using ERPSystem.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace ERPSystem.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    //[Route("Transactions/[controller]/[action]")]
    public class SalesController : BaseController
    {
        [BindProperty]
        public SalesOrderViewModel PurOrdVM { get; set; }
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _config;
        public SalesController(UserManager<IdentityUser> userManager, IConfiguration config, JwtSettings jwtSettings) : base(userManager)
        {
            _jwtSettings = jwtSettings;
            _config = config;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
        {
            SalesOrderViewModel orderData = new SalesOrderViewModel();
            if (ModelState.IsValid)
            {
                orderData.order.DateSales = PurOrdVM.order.DateSales;
                orderData.order.Carriage = PurOrdVM.order.Carriage;
                orderData.order.Labour = PurOrdVM.order.Labour;
                orderData.order.DocumentNumber = PurOrdVM.order.DocumentNumber;
                orderData.order.TotalExpenses = PurOrdVM.order.TotalExpenses;
                orderData.order.Cutting = PurOrdVM.order.Cutting;
                orderData.order.Carrogation = PurOrdVM.order.Carrogation;
                orderData.order.Misc = PurOrdVM.order.Misc;
                orderData.order.Loading = PurOrdVM.order.Loading;
                orderData.order.Carriage = PurOrdVM.order.Carriage;
                orderData.order.PayingAmount = PurOrdVM.order.PayingAmount;
                orderData.order.GrandTotal = PurOrdVM.order.GrandTotal;
                orderData.order.InvoiceAmount = PurOrdVM.order.InvoiceAmount;
                orderData.order.Notes = PurOrdVM.order.Notes;
                orderData.order.ReferenceNum = PurOrdVM.order.ReferenceNum;
                orderData.order.AccountCodeId = PurOrdVM.order.AccountCodeId;
                orderData.order.DateTimeEntered = DateTime.Now;
                orderData.order.DueDate = PurOrdVM.order.DueDate;
                orderData.order.SalesGoodsId = PurOrdVM.order.SalesGoodsId;
                orderData.items = PurOrdVM.items;

                await Create(orderData);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(orderData);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSalesOrders()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/sales/getall";
                var res = await ApiCalls<SaleNoteListVM>.GetData(apiUrl);
                return Json(new
                {
                    data = res
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(SalesOrderViewModel ord)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/sales/";
            var res = await ApiCalls<SalesOrderViewModel>.Add(ord, apiUrl);
            return Json(new
            {
                data = res
            });
        }
    }
}
