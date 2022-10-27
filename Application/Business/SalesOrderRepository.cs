using Application.Business.Transactions.IAppServices;
using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel.Reports;
using Domain.ViewModel.Transactions;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Transactions
{
    public class SalesOrderRepository : Repository<SalesGoods>, ISalesOrderRepository
    {
        private readonly ERPDBContext db;

        public SalesOrderRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<List<SaleNoteListVM>> GetSaleOrders()
        {
            var item = await db.SalesGood
                .OrderByDescending(a => a.SalesGoodsId)
                .Select(b => new SaleNoteListVM { 
                    SalesGoodsId = b.SalesGoodsId,
                    CustomerName = b.AccountCode.Description,
                    DateSales = b.DateSales,
                    DocumentNumber = b.DocumentNumber,
                    DueDate = b.DueDate,
                    GrandTotal = b.GrandTotal,
                    InvoiceAmount = b.InvoiceAmount,
                    PayingAmount = b.PayingAmount,
                    TotalExpenses = b.TotalExpenses
                })
                .ToListAsync(); ;
            return item;
        }
        public async Task<bool> PlaceSaleOrder(SalesOrderViewModel purOrder)
        {
            int? weight = 0;
            decimal? price = 0;
            decimal? totSal = 0;

            string desc = "<h5>Sales Note </h5> <table class='table table-striped table-responsive table-bordered'style='font-size:12px'><thead><tr><th>Item</th><th>Rate</th><th>Weight</th><th>Total</th></tr></thead><tbody>";

            bool addStatus = false;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    int dayOpened = await db.dayOpening.Where(c => c.closeStatus == false).Select(k => k.DayOpeningsId).SingleOrDefaultAsync();
                    
                    SalesGoods ord = new SalesGoods();
                    ord.DateSales = purOrder.order.DateSales;
                    ord.Carriage = purOrder.order.Carriage;
                    ord.DocumentNumber = purOrder.order.DocumentNumber;
                    ord.TotalExpenses = purOrder.order.TotalExpenses;
                    ord.Labour = purOrder.order.Labour;
                    ord.DayOpeningsId = dayOpened;
                    ord.Cutting = purOrder.order.Cutting;
                    ord.Carrogation = purOrder.order.Carrogation;
                    ord.Misc = purOrder.order.Misc;
                    ord.Loading = purOrder.order.Loading;
                    ord.Carriage = purOrder.order.Carriage;
                    ord.PayingAmount = purOrder.order.PayingAmount;
                    ord.GrandTotal = purOrder.order.GrandTotal;
                    ord.InvoiceAmount = purOrder.order.InvoiceAmount;
                    ord.Notes = purOrder.order.Notes;
                    ord.ReferenceNum = purOrder.order.ReferenceNum;
                    ord.AccountCodeId = purOrder.order.AccountCodeId;
                    ord.Acknowledged = false;
                    ord.DueDate = purOrder.order.DueDate;
                    ord.DateTimeEntered = DateTime.Now;
                    Add(ord);
                    db.SaveChanges();

                    foreach (var itm in purOrder.items)
                    {
                        SalesItems purItems = new SalesItems();
                        purItems.ItemsId = itm.ItemsId;
                        purItems.SalesGoodsOrderId = ord.SalesGoodsId;
                        purItems.Qty = itm.Qty;
                        purItems.SalePrice = itm.SalePrice;
                        purItems.Notes = itm.Notes;
                        purItems.DateTimeEntered = DateTime.Now;
                        db.SalesItem.Add(purItems);
                        db.SaveChanges();

                        ItemStock stock = db.ItemStocks.Where(a => a.ItemId == itm.ItemsId).FirstOrDefault();
                        stock.Qty -= Convert.ToInt32(itm.Qty);
                        db.SaveChanges();
                    }

                    var create = await db.SaveChangesAsync();
                    if (create > 0)
                    {
                        addStatus = true;
                    }

                    AccountCode accBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                    accBal.OpeningBalance = accBal.OpeningBalance - ord.TotalExpenses - (ord.InvoiceAmount - ord.PayingAmount);
                    db.SaveChanges();

                    dbContextTransaction.Commit();

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
                return addStatus;
            }
        }     
    }
}