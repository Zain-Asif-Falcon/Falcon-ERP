using Application;
using Domain.Entities;
using Domain.ViewModel.API;
using Domain.ViewModel.Transactions;
using ERPSystemAPI.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystemAPI.Controllers.V1.Transactions
{
    public class PurchasesController : Controller
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<PurchasesController> _logger;
        public PurchasesController(ILogger<PurchasesController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.Purchases.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _IUnitOfWork.purchOrder.GetPurchaseOrders();
            return Ok(categories);
        }
        [HttpGet(ApiRoutes.Purchases.Get)]
        public async Task<IActionResult> Get(int purchaseId)
        {
            if (purchaseId >= 0)
            {
                var catInfo = await _IUnitOfWork.purchOrder.GetPurchaseGoodInfo(purchaseId);
                return Ok(catInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Order Id" }
                });
            }
        }
        [HttpPost(ApiRoutes.Purchases.Create)]
        public async Task<IActionResult> PlaceOrder([FromBody] PurchaseOrderViewModel ord)
        {
            try
            {
                var placeOrder = await _IUnitOfWork.purchOrder.PlacePurchaseOrder(ord);
                return Ok(placeOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut(ApiRoutes.Purchases.Update)]
        public async Task<IActionResult> UpdatePlaceOrder([FromBody] PurchaseOrderViewModel ord)
        {
            try
            {
                var placeOrder = await _IUnitOfWork.purchOrder.EditPlacePurchaseOrder(ord);
                return Ok(placeOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        //===================================================
        [HttpGet(ApiRoutes.Purchases.GetPurchaseItems)]
        public async Task<IActionResult> GetPurchaseItems(int purchaseId = 0)
        {
            var categories = await _IUnitOfWork.purchOrder.GetPurchaseItemsInfo(purchaseId);
            return Ok(categories);
        }
        
        //==================== Report ===============================
        [HttpGet(ApiRoutes.Purchases.GetRepo)]
        public async Task<IActionResult> GetRepo(int purchaseId)
        {
            if (purchaseId >= 0)
            {
                var catInfo = await _IUnitOfWork.purchOrder.GetReportPurchaseGoodInfo(purchaseId);
                return Ok(catInfo);
            }
            else
            {
                return BadRequest(new GenericRequestResponse
                {
                    ErrorMessage = new[] { "Please provide Order Id" }
                });
            }
        }
        [HttpGet(ApiRoutes.Purchases.GetPurchaseRepoItems)]
        public async Task<IActionResult> GetReportPurchaseItems(int purchaseId = 0)
        {
            var categories = await _IUnitOfWork.purchOrder.GetReportPurchaseItemsInfo(purchaseId);
            return Ok(categories);
        }

        [HttpGet(ApiRoutes.Purchases.GetDateWisePurchasesRepo)]
        public async Task<IActionResult> GetDateWisePurchases(DateTime date)
        {
            var categories = await _IUnitOfWork.purchOrder.GetDateWisePurchases(date);
            return Ok(categories);
        }
    }
}
