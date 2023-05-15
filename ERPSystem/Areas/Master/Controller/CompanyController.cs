using Application.Common.Utilities;
using Domain.Entities;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystem.Areas.Master.Controllers
{
    [Area("Master")]
    public class CompanyController : Controller
    {
        [BindProperty]
        public CompanyViewModel cmpVM { get; set; }
        private readonly IConfiguration _config;
        public CompanyController(IConfiguration config)
        {
            _config = config;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            cmpVM = new CompanyViewModel();

            if (id != null)
            {
                var compny = await GetById((int)id);
                cmpVM.comp.CompanyId = compny.CompanyId;
                cmpVM.comp.Address = compny.Address;
                cmpVM.comp.NTN = compny.NTN;
                cmpVM.comp.phone = compny.phone;
                cmpVM.comp.STRN = compny.STRN;
                cmpVM.comp.Name = compny.Name;
            }

            return View(cmpVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {

            if (ModelState.IsValid)
            {
                Company accC = new Company();
                accC.CompanyId = cmpVM.comp.CompanyId;
                accC.Address = cmpVM.comp.Address;
                accC.Name = cmpVM.comp.Name;
                accC.NTN = cmpVM.comp.NTN;
                accC.phone = cmpVM.comp.phone;
                accC.STRN = cmpVM.comp.STRN;
                if (cmpVM.comp.CompanyId == 0)
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
                return View(cmpVM);
            }
        }
        #region API Calling

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/company/getall";
            var res = await ApiCalls<Company>.GetData(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/company/getactives";
            var res = await ApiCalls<Company>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNonActives()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/company/getnonactives";
            var res = await ApiCalls<Company>.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }

        [HttpPut]
        public async Task<IActionResult> Delete(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/company/{Id}";
            var res = await ApiCalls<Company>.Remove(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Company accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/company/";
            var res = await ApiCalls<Company>.Add(accC, apiUrl);
            return Json(new
            {
                data = res
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(Company accH)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/company/update";
            var res = await ApiCalls<Company>.Update(accH, apiUrl);
            return Json(new
            {
                data = res
            });
        }

       
        [HttpGet]
        public async Task<IActionResult> CheckExisting(string Name)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/company/checkname/{Name}";
            var res = await ApiCalls<Company>.CheckExisting(apiUrl);
            //await _serviceCaller.GetData(apiUrl);
            return Json(new
            {
                data = res
            });
        }
        #endregion
    }
}
