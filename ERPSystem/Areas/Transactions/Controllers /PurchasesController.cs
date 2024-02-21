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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystem.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class PurchasesController : BaseController
    {
        [BindProperty]
        public PurchaseOrderViewModel PurOrdVM { get; set; }
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _config;
        public PurchasesController(UserManager<IdentityUser> userManager, IConfiguration config, JwtSettings jwtSettings) : base(userManager)
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
            ViewBag.Product =  await ProductsDropDownData(); 
            return PartialView("_PurchaseOrderDetails", new Domain.Entities.PurchaseItem());
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
            PurOrdVM = new PurchaseOrderViewModel()
            {
                order = new PurchaseGood(),
                items = new List<PurchaseItem>()                
            };
            if (id == 0)
            {
                PurOrdVM.order.PurchasesGoodsId = 0;
            }
            if (id == null)
            {
                return View(PurOrdVM);
            }
            if (PurOrdVM.order == null)
            {
                PurOrdVM.order = new PurchaseGood();
            }
            ViewBag.Product = await ProductsDropDownData();
            ViewBag.AccountCodes = await AccountCodesDropDownData();
            if (id >= 0)
            {
                PurOrdVM.order = (PurchaseGood)await GetPurchaseOrdersView(id.GetValueOrDefault());// _IUnitOfWork.orders.GetPurchaseGoodInfo(id.GetValueOrDefault());
                PurOrdVM.items = (List<PurchaseItem>)await GetPurchaseItems((int)id);
                if (id > 0)
                    PurOrdVM.partyDet = (PartyCodes)await GetPartyByIdRepo((int)PurOrdVM.order.AccountCodeId);
            }
            if (id == 0)
            {
                PurchaseGood orderDat = new PurchaseGood();
                if (PurOrdVM.order != null)
                {
                    PurOrdVM.order.PurchasesGoodsId = 0;
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
                    PurchaseGood sl = new PurchaseGood();
                    PurOrdVM.order = sl;
                    PurOrdVM.order.PurchasesGoodsId = 0;
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
                PurOrdVM.order = orderDat;
            }
            if (PurOrdVM == null)
            {
                return NotFound();
            }
            return await Task.Run(() => View(PurOrdVM));
        }

        #region API Calling
        [HttpGet]
        public async Task<PurchaseGood> GetPurchaseOrdersView(int id)
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/purchases/{id}";
                Task<PurchaseGood> res = ApiCalls<PurchaseGood>.GetById(apiUrl);
                return await res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            try
            {
                string apiUrl = _config["ServerAddress:Address"] + $"/api/v1/purchases/getall";
                var res = await ApiCalls<PurchaseNoteListVM>.GetData(apiUrl);
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
   
      
    }
}
