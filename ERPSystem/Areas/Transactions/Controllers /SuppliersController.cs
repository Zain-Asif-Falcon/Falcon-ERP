using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Utilities;
using Domain.Entities;
using Domain.ViewModel.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace ERPSystem.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SuppliersController : Controller
    {
        [BindProperty]
        public SupplierViewModel AccCodeVM { get; set; }
        private readonly IConfiguration _config;
        public SuppliersController(IConfiguration config)
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
            AccCodeVM = new SupplierViewModel();
            ViewBag.AccountHeads = await DropDownData();
            if (id != null)
            {
                var AccCodes = await GetById((int)id);
                AccCodeVM.supp.SuppliersId = AccCodes.SuppliersId;
                AccCodeVM.supp.Address = AccCodes.Address;
                AccCodeVM.supp.Email = AccCodes.Email;
                AccCodeVM.supp.MobileNumber = AccCodes.MobileNumber;
                AccCodeVM.supp.Name = AccCodes.Name;
                AccCodeVM.supp.NtaxNumber = AccCodes.NtaxNumber;
            }

            return View(AccCodeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                Supplier accC = new Supplier();
                accC.SuppliersId = AccCodeVM.supp.SuppliersId;
                accC.Name = AccCodeVM.supp.Name;
                accC.Address = AccCodeVM.supp.Address;
                accC.Email = AccCodeVM.supp.Email;
                accC.MobileNumber = AccCodeVM.supp.MobileNumber;
                accC.NtaxNumber = AccCodeVM.supp.NtaxNumber;
                accC.IsActive = true;
                accC.created_at = DateTime.Now;
                if (AccCodeVM.supp.SuppliersId == 0)
                {
                    await Create(accC);
                }
                else
                {
                    await Update(accC);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(AccCodeVM);
            }
        }
        #region API Calling      
        [HttpPost]
        public async Task<IActionResult> Create(Supplier accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/suppliers/";
            var res = await ApiCalls<Supplier>.Add(accC, apiUrl);
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
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/suppliers/dropdown/";
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
