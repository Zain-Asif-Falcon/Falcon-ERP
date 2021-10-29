using Application.Common.Utilities;
using Domain.Contracts.V1;
using Domain.Entities;
using Domain.ViewModel.Dashboard;
using ERPSystem.Extensions;
using ERPSystem.Models;
using Jose;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IConfiguration config) : base(userManager)
        {
            _config = config;
            _userManager = userManager;
            _logger = logger;
        }       
        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            var email = user.Result.Email;
            HttpContext.Session.SetObject(Domain.Utility.SD.SessionCurrentUser, email);

            //TicketsViewModel tiles = await GetTicketsCount();

            //ViewBag.CompanyTicket = tiles.companies;
            //ViewBag.ItemTicket = tiles.items;
            //ViewBag.PartyTicket = tiles.parties;
            //ViewBag.SupplierTicket = tiles.suppliers;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region API Calling

        [HttpGet]
        public async Task<TicketsViewModel> GetTicketsCount()
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/dashboard/GetTicketsCounts";
            var res = await ApiCalls<TicketsViewModel>.GetById(apiUrl);
            return res;
        }
        #endregion
    }
}
