using Application;
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
    public class SalesController : Controller
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<SalesController> _logger;
        public SalesController(ILogger<SalesController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }
        [HttpGet(ApiRoutes.Sales.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _IUnitOfWork.saleOrder.GetSaleOrders();
            return Ok(categories);
        }
        [HttpPost(ApiRoutes.Sales.Create)]
        public async Task<IActionResult> PlaceOrder([FromBody] SalesOrderViewModel ord)
        {
            try
            {
                var placeOrder = await _IUnitOfWork.saleOrder.PlaceSaleOrder(ord);
                return Ok(placeOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
