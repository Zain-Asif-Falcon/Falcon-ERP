using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Utilities;
using Domain.Contracts.V1;
using Domain.Entities;
using Domain.ViewModel.Options;
using Domain.ViewModel.Reports;
using Domain.ViewModel.Transactions;
using ERPSystem.Controllers;
using ERPSystem.Extensions;
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> AddItem()
        {
            ViewBag.Product = await ProductsDropDownData();
            return PartialView("_SaleOrderDetails", new Domain.ViewModel.Transactions.saleItemsVM());
        }
        //[HttpDelete]
        //public IActionResult RemoveItem(int purchaseOrderId, int productId)
        //{
        //    var objFromDb = _IUnitOfWork.ordersItems.GetAll(filter: x => x.PurchaseItemsId == purchaseOrderId && x.ProductId == productId).FirstOrDefault();
        //    if (objFromDb == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting." });
        //    }
        //    else
        //    {
        //        _IUnitOfWork.ordersItems.Remove(objFromDb);
        //        _IUnitOfWork.Save();
        //        return Json(new { success = true, message = "Deleted successful." });
        //    }
        //}
        public async Task<IActionResult> PlaceOrder(int? id = 0)
        {
            PurOrdVM = new SalesOrderViewModel()
            {
                order = new SalesGoods(),
                items = new List<saleItemsVM>(),
                partyDet = new PartyCodes()
            };
            if (id == null)
            {
                return View(PurOrdVM);
            }
            if (PurOrdVM.order == null)
            {
                PurOrdVM.order = new SalesGoods();
            }
            ViewBag.Product = await ProductsDropDownData();
            ViewBag.AccountCodesCode = await AccountCodesDropDownCode();
            ViewBag.AccountCodesName = await AccountCodesDropDownName();
            if (id >= 0)
            {
                PurOrdVM.order = (SalesGoods)await GetSalesOrdersView(id.GetValueOrDefault());// _IUnitOfWork.orders.GetPurchaseGoodInfo(id.GetValueOrDefault());
                PurOrdVM.items = (List<saleItemsVM>)await GetSalesItems((int)id);
                if(id > 0)
                    PurOrdVM.partyDet = (PartyCodes)await GetPartyByIdRepo((int)PurOrdVM.order.AccountCodeId);
            }
            if (id == 0)
            {
                SalesGoods orderDat = new SalesGoods();
                if(PurOrdVM.order != null)
                {
                    PurOrdVM.order.SalesGoodsId = 0;
                    if (PurOrdVM.order.DocumentNumber != null)
                    {
                        //PurOrdVM.order.DocumentNumber += 1;
                        orderDat.DocumentNumber = PurOrdVM.order.DocumentNumber;
                    }
                    else
                    {
                        PurOrdVM.order.DocumentNumber = "001-001";
                        orderDat.DocumentNumber = PurOrdVM.order.DocumentNumber;
                    }
                }
                else
                {
                    SalesGoods sl = new SalesGoods();
                    PurOrdVM.order = sl;
                    PurOrdVM.order.SalesGoodsId = 0;
                    if (PurOrdVM.order.DocumentNumber != null)
                    {
                        PurOrdVM.order.DocumentNumber += 1;
                        orderDat.DocumentNumber = PurOrdVM.order.DocumentNumber;
                    }
                    else
                    {
                        PurOrdVM.order.DocumentNumber = "001-000";
                        orderDat.DocumentNumber = PurOrdVM.order.DocumentNumber;
                    }
                }
                PurOrdVM.order = orderDat;
            }
            if (PurOrdVM == null)
            {
                return NotFound();
            }
            return await Task.Run(() => View(PurOrdVM));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        [HttpGet]
        public async Task<IEnumerable<saleItemsVM>> GetSalesItems(int id = 0)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/saleItems/{id}";
            Task<IEnumerable<saleItemsVM>> res;
            res = ApiCalls<saleItemsVM>.GetDataById(apiUrl);
            return await res;
        }
        [HttpPost]
        public async Task<IActionResult> Create(SalesOrderViewModel ord)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/sales/";
            var res = await ApiCalls<SalesOrderViewModel>.AddResponseMessage(ord, apiUrl);
            return Json( res);
        }
        [HttpPut]
        public async Task<IActionResult> Update(SalesOrderViewModel ord)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/sales/update";
            var res = await ApiCalls<SalesOrderViewModel>.UpdateResponseMessage(ord, apiUrl);
            return Json( res);
        }
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> ProductsDropDownData()
        {
            try
            {
                var user = getLoggedInUser().Result;

                string secret = _jwtSettings.Secret;
                string issuer = _config["JwtSettings:Issuer"];

                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/items/dropdown/";
                string res = await ApiCalls<string>.GetDropDownListWithHeaders(apiUrl, user, secret, issuer);
                List<SelectListItem> record = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(res);
                return record;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
       
        [HttpGet]
        public async Task<PartyCodes> GetPartyByIdRepo(int Id)
        {
            string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/partycodeRepo/{Id}";
            Task<PartyCodes> res;
            res = ApiCalls<PartyCodes>.GetById(apiUrl);
            return await res;
        }

       
        #endregion
    }
}
