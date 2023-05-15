using Application.Common.Utilities;
using Domain.Entities;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystem.Areas.Master.Controllers
{
    [Area("Master")]
    public class AccountCodeController : Controller
    {
        [BindProperty]
        public AccountCodeViewModel AccCodeVM { get; set; }
        private readonly IConfiguration _config;
        public AccountCodeController(IConfiguration config)
        {
            _config = config;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            AccCodeVM = new AccountCodeViewModel();
            ViewBag.AccountHeads = await DropDownData();
            if (id != null)
            {
                var AccCodes = await GetById((int)id);
                AccCodeVM.AccCode.AccountCodeId = AccCodes.AccountCodeId;
                AccCodeVM.AccCode.AccountHeadId = AccCodes.AccountHeadId;
                AccCodeVM.AccCode.Code = AccCodes.Code;
                AccCodeVM.AccCode.OpeningBalance = AccCodes.OpeningBalance;
                AccCodeVM.AccCode.Description = AccCodes.Description;
            }

            return View(AccCodeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                AccountCode accC = new AccountCode();
                accC.AccountCodeId = AccCodeVM.AccCode.AccountCodeId;
                accC.AccountHeadId = AccCodeVM.AccCode.AccountHeadId;
                accC.Code = accC.AccountHeadId.ToString("D3") +"-"+AccCodeVM.AccCode.Code;
                accC.OpeningBalance = AccCodeVM.AccCode.OpeningBalance;
                accC.Description = AccCodeVM.AccCode.Description;
                if (AccCodeVM.AccCode.AccountCodeId == 0)
                {
                    accC.IsActive = true;
                    accC.created_at = DateTime.Now;
                    await Create(accC);
                }
                else
                {
                    accC.updated_at = DateTime.Now;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/getall";
            var res = await ApiCalls<AccountCodeViewModel>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/getactives";
            var res = await ApiCalls<AccountCodeViewModel>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/getnonactives";
            var res = await ApiCalls<AccountCode>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/{Id}";
            var res = await ApiCalls<AccountCode>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(AccountCode accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/";
            var res = await ApiCalls<AccountCode>.Add(accC, apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(AccountCode accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/update";
            var res = await ApiCalls<AccountCode>.Update(accH, apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<AccountCode> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/{Id}";
            Task<AccountCode> res;
            res = ApiCalls<AccountCode>.GetById(apiUrl);
            return await res;
        }
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> DropDownData()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountheads/dropdown/";
                string res = await ApiCalls<string>.GetDropDownList(apiUrl);
                List<SelectListItem> record = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(res);
                return record;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> CheckExistingCode(string Code)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/CheckCode/{Code}";
            var res = await ApiCalls<AccountCode>.CheckExisting(apiUrl);           
            return Json(new
            {
                data = res
            });
        }
        #endregion
    }
}
