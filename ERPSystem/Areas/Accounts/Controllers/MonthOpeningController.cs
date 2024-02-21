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
        [HttpPost]
        public async Task<bool> Create(MonthOpenings accC)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/monthopenings/";
            var res = await ApiCalls<MonthOpenings>.Add(accC, apiUrl);
            return res;
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
