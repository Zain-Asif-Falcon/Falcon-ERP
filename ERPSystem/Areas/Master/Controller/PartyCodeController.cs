using Application.Common.Utilities;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.Reports;
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
    public class PartyCodeController : Controller
    {
        [BindProperty]
        public PartyCodeViewModel PartCodeVM { get; set; }
        private readonly IConfiguration _config;
        public PartyCodeController(IConfiguration config)
        {
            _config = config;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            ViewBag.AccountCodes = await AccountCodesDropDownData();
            PartCodeVM = new PartyCodeViewModel();

            if (id != null)
            {
                var prtCodes = await GetById((int)id);
                PartCodeVM.partCode.PartyCodeId = prtCodes.PartyCodeId;
                PartCodeVM.partCode.AccountCodeId = prtCodes.AccountCodeId;
                PartCodeVM.partCode.Code = prtCodes.Code;
                PartCodeVM.partCode.Address = prtCodes.Address;
                PartCodeVM.partCode.city = prtCodes.city;
                PartCodeVM.partCode.contactPerson = prtCodes.contactPerson;
                PartCodeVM.partCode.creditDays = prtCodes.creditDays;
                PartCodeVM.partCode.fax = prtCodes.fax;
                PartCodeVM.partCode.AccountDate = prtCodes.AccountDate;
                PartCodeVM.partCode.Name = prtCodes.Name;
                PartCodeVM.partCode.phone = prtCodes.phone;
                PartCodeVM.partCode.STRN = prtCodes.STRN;
                PartCodeVM.partCode.remarks = prtCodes.remarks;
                PartCodeVM.partCode.MobileOne = prtCodes.MobileOne;
                PartCodeVM.partCode.MobileTwo = prtCodes.MobileTwo;
                PartCodeVM.partCode.IsActive = prtCodes.IsActive;
                PartCodeVM.partCode.NTN = prtCodes.NTN;
            }

            return View(PartCodeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                PartyCode partC = new PartyCode();
                partC.AccountCodeId = PartCodeVM.partCode.AccountCodeId;
                partC.PartyCodeId = PartCodeVM.partCode.PartyCodeId;
                partC.Code = PartCodeVM.partCode.Code;
                partC.Address = PartCodeVM.partCode.Address;
                partC.city = PartCodeVM.partCode.city;
                partC.contactPerson = PartCodeVM.partCode.contactPerson;
                partC.creditDays = PartCodeVM.partCode.creditDays;
                partC.fax = PartCodeVM.partCode.fax;
                partC.Name = PartCodeVM.partCode.Name;
                partC.AccountDate = PartCodeVM.partCode.AccountDate;
                partC.phone = PartCodeVM.partCode.phone;
                partC.STRN = PartCodeVM.partCode.STRN;
                partC.remarks = PartCodeVM.partCode.remarks;
                partC.MobileOne = PartCodeVM.partCode.MobileOne;
                partC.MobileTwo = PartCodeVM.partCode.MobileTwo;
                partC.IsActive = PartCodeVM.partCode.IsActive;
                partC.NTN = PartCodeVM.partCode.NTN;
                if (PartCodeVM.partCode.PartyCodeId == 0)
                {
                    partC.IsActive = true;
                    partC.created_at = DateTime.Now;
                    await Create(partC);
                }
                else
                {
                    partC.updated_at = DateTime.Now;
                    await Update(partC);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(PartCodeVM);
            }
        }
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/getall";
            var res = await ApiCalls<PartyCode>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/getactives";
            var res = await ApiCalls<PartyCodes>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/getnonactives";
            var res = await ApiCalls<PartyCode>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/{Id}";
            var res = await ApiCalls<PartyCode>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(PartyCode accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/";
            var res = await ApiCalls<PartyCode>.Add(accC, apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(PartyCode accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/update";
            var res = await ApiCalls<PartyCode>.Update(accH, apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpGet]
        public async Task<PartyCode> GetById(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/{Id}";
            Task<PartyCode> res;
            res = ApiCalls<PartyCode>.GetById(apiUrl);
            return await res;
        }

        [HttpGet]
        public async Task<IActionResult> CheckExisting(string Name)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycode/checkname/{Name}";
            var res = await ApiCalls<PartyCode>.CheckExisting(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> AccountCodesDropDownData()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/accountcodes/dropdown/";
                string res = await ApiCalls<string>.GetDropDownList(apiUrl);
                List<SelectListItem> record = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(res);
                return record;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //=============================================================
        [HttpGet]
        public async Task<PartyCodes> GetByIdRepo(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycodeRepo/{Id}";
            Task<PartyCodes> res;
            res = ApiCalls<PartyCodes>.GetById(apiUrl);
            return await res;
        }
        #endregion
    }
}
