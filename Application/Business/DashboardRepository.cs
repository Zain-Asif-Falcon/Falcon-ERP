using Application.Business.IAppServices;
using Domain.Entities;
using Domain.ViewModel.Dashboard;
using Domain.ViewModel.Reports;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ERPDBContext db;

        public DashboardRepository(ERPDBContext _db, ILogger logger)
        {
            this.db = _db;
        }

        public async Task<LatestFiveRecordsViewModel> GetLastFiveList()
        {
            LatestFiveRecordsViewModel list = new LatestFiveRecordsViewModel
            {
                sales =await db.SalesGood
                .OrderByDescending(a => a.SalesGoodsId)
                .Select(b => new SalesNote
                {
                    DateSales = b.DateSales,
                    GrandTotal = b.GrandTotal,
                    DocumentNumber = b.DocumentNumber,
                    Code = b.AccountCode.Code,
                    Description = b.AccountCode.Description,
                    TotalExpenses = b.TotalExpenses
                }).Take(5)
                .ToListAsync(),
                
                purchase = await db.PurchaseGoods
                .OrderByDescending(a => a.PurchasesGoodsId)
                .Select(b => new PurchasesNote
                {
                    DatePurchase = b.DatePurchase,
                    GrandTotal = b.GrandTotal,
                    DocumentNumber = b.DocumentNumber,
                    Code = b.AccountCode.Code,
                    Description = b.AccountCode.Description,
                    TotalExpenses = b.TotalExpenses
                }).Take(5)
                .ToListAsync(),
                
                cashPay = await db.CashPayingItems
                .OrderByDescending(a => a.CashPaymentId)
                .Take(5)
                .Select(b => new CashPaymentItemsRepo
                {
                  accountCode = b.AccountCode.Code,
                  accountCodeName = b.AccountCode.Description,
                  debit = b.debit,
                  description = b.Description,
                  datePayment = b.created_at
                }).ToListAsync(),
                
                cashRec = await db.cashRecievingItems
                .OrderByDescending(a => a.CashRecievedId)
                .Take(5)
                .Select(b => new CashRecieptItems
                {
                    accountCode = b.AccountCode.Code,
                    accountCodeName = b.AccountCode.Description,
                    credit = b.credit,
                    description = b.Description,
                    datePayed = b.created_at
                }).ToListAsync()
            };
            return list;
        }
        public async Task<MonthlyTotals> GetMonthlyTotals(DateTime dat)
        {
            MonthlyTotals result = new MonthlyTotals
            {
                salesCash = (decimal)await db.SalesGood.Where(a => a.DateSales.Value.Year == dat.Year && a.DateSales.Value.Month == dat.Month && a.GrandTotal == a.PayingAmount).Select(b => b.GrandTotal).SumAsync(),
                salesCredit = (decimal)await db.SalesGood.Where(a => a.DateSales.Value.Year == dat.Year && a.DateSales.Value.Month == dat.Month && a.GrandTotal != a.PayingAmount).Select(b => b.PayingAmount).SumAsync(),
                purchasesCash = (decimal)await db.PurchaseGoods.Where(a => a.DatePurchase.Value.Year == dat.Year && a.DatePurchase.Value.Month == dat.Month && a.GrandTotal == a.PayingAmount).Select(b => b.PayingAmount).SumAsync(),
                purchasesCredit = (decimal)await db.PurchaseGoods.Where(a => a.DatePurchase.Value.Year == dat.Year && a.DatePurchase.Value.Month == dat.Month && a.GrandTotal != a.PayingAmount).Select(b => b.PayingAmount).SumAsync(),
                payments = (decimal)await db.CashPayingItems.Where(a => a.CashPayment.documentDate.Value.Year == dat.Year && a.CashPayment.documentDate.Value.Month == dat.Month).Select(b => b.debit).SumAsync(),
                reciepts = (decimal)await db.cashRecievingItems.Where(a => a.CashRecieved.documentDate.Value.Year == dat.Year && a.CashRecieved.documentDate.Value.Month == dat.Month).Select(b => b.credit).SumAsync()
            };
            return result;
        }
        public async Task<GraphRecordsViewModel> GetGraphWeeklyList()
        {
            List<GraphRecordVM> data = new List<GraphRecordVM>();
            
            for(int i=-6;i<=0;i++)
            {
                DateTime dat = DateTime.Now.AddDays(i);

                var t1 = (decimal)await db.SalesGood
                .Where(a => a.DateSales.Value.Year == dat.Year
                    && a.DateSales.Value.Month == dat.Month
                    && a.DateSales.Value.Day == dat.Day)
                .Select(b => b.GrandTotal).SumAsync();

                var t2 = (decimal)await db.PurchaseGoods
                    .Where(a => a.DatePurchase.Value.Year == dat.Year
                        && a.DatePurchase.Value.Month == dat.Month
                        && a.DatePurchase.Value.Day == dat.Day)
                    .Select(b => b.GrandTotal).SumAsync();

                var t3 = (decimal)await db.CashPayingItems
                    .Where(a => a.CashPayment.documentDate.Value.Year == dat.Year
                        && a.CashPayment.documentDate.Value.Month == dat.Month
                        && a.CashPayment.documentDate.Value.Day == dat.Day)
                    .Select(b => b.debit).SumAsync();

                var t4 = (decimal)await db.cashRecievingItems
                    .Where(a => a.CashRecieved.documentDate.Value.Year == dat.Year
                        && a.CashRecieved.documentDate.Value.Month == dat.Month
                        && a.CashRecieved.documentDate.Value.Day == dat.Day)
                    .Select(b => b.credit).SumAsync();

                GraphRecordVM dataa = new GraphRecordVM
                {
                    GrandSalesTotal = t1,
                    GrandPurchasesTotal = t2,
                    GrandPaymentTotal = t3,
                    GrandRecieptsTotal = t4,
                    TransactionDate = dat
                };
                data.Add(dataa);
            }
            
            GraphRecordsViewModel list = new GraphRecordsViewModel
            {
                data = data.OrderByDescending(l => l.TransactionDate).ToList()
            };
            return list;
        }
        
        public async Task<TicketsViewModel> GetTicketsCounts()
        {
            TicketsViewModel result = new TicketsViewModel 
            { 
                items = await db.Items.Where(a => a.IsActive == true).CountAsync(), 
                accountCodes = await db.AccountCodes.Where(a => a.IsActive == true).CountAsync(), 
                accountHeads = await db.AccountHeads.Where(a => a.IsActive == true).CountAsync()
            };
            return result;
        }
    }
}
