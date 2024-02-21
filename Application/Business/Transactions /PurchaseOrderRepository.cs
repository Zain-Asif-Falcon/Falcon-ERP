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
    public class PurchaseOrderRepository : Repository<PurchaseGood>, IPurchaseOrderRepository
    {
        private readonly ERPDBContext db;

        public PurchaseOrderRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<List<PurchaseNoteListVM>> GetPurchaseOrders()
        {
            var item = await db.PurchaseGoods
                .OrderByDescending(a => a.PurchasesGoodsId)
                .Select(b => new PurchaseNoteListVM
                {
                    PurchasesGoodsId = b.PurchasesGoodsId,
                    CustomerName = b.AccountCode.Description,
                    DateSales = b.DatePurchase,
                    DocumentNumber = b.DocumentNumber,
                    DueDate = b.DueDate,
                    GrandTotal = b.GrandTotal,
                    InvoiceAmount = b.InvoiceAmount,
                    PayingAmount = b.PayingAmount,
                    TotalExpenses = b.TotalExpenses
                })
                .ToListAsync();
            return item;
        }     
       
        public async Task<bool> PlacePurchaseOrder(PurchaseOrderViewModel purOrder)
        {
            int? weight = 0;
            string desc = "Purchase Note \n";

            bool addStatus = false;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    int dayOpened = await db.dayOpening.Where(c => c.closeStatus == false).Select(k => k.DayOpeningsId).SingleOrDefaultAsync();

                    PurchaseGood ord = new PurchaseGood();
                    ord.DatePurchase = purOrder.order.DatePurchase;
                    ord.Carriage = purOrder.order.Carriage;
                    ord.DocumentNumber = purOrder.order.DocumentNumber;
                    ord.TotalExpenses = purOrder.order.TotalExpenses;
                    ord.Labour = purOrder.order.Labour;
                    ord.DayOpeningsId = dayOpened;
                    ord.Cutting = purOrder.order.Cutting;
                    ord.Carrogation = purOrder.order.Carrogation;
                    ord.Misc = purOrder.order.Misc;
                    ord.Loading = purOrder.order.Loading;
                    ord.PayingAmount = purOrder.order.PayingAmount;
                    ord.GrandTotal = purOrder.order.GrandTotal;
                    ord.InvoiceAmount = purOrder.order.InvoiceAmount;
                    ord.Notes = purOrder.order.Notes;
                    ord.ReferenceNum = purOrder.order.ReferenceNum;
                    ord.AccountCodeId = purOrder.order.AccountCodeId;
                    ord.DueDate = purOrder.order.DueDate;
                    ord.Acknowledged = false;
                    ord.DateTimeEntered = DateTime.Now;
                    Add(ord);
                    db.SaveChanges();
                    foreach (var itm in purOrder.items)
                    {
                        PurchaseItem purItems = new PurchaseItem();
                        purItems.ItemsId = itm.ItemsId;
                        purItems.PurchaseGoodsOrderId = ord.PurchasesGoodsId;
                        purItems.PurchaseRate = itm.PurchaseRate;
                        purItems.Qty = itm.Qty;
                        purItems.Notes = itm.Notes;
                        purItems.UnitPrice = itm.UnitPrice;
                        purItems.DateTimeEntered = DateTime.Now;
                        db.PurchaseItems.Add(purItems);
                        db.SaveChanges();

                        //weight += purItems.Qty;

                        ItemStock stock = db.ItemStocks.Where(a => a.ItemId == itm.ItemsId).FirstOrDefault();
                        if (stock == null)
                        {
                            ItemStock st = new ItemStock();
                            st.Qty = (int)itm.Qty;
                            st.ItemId = (int)itm.ItemsId;
                            db.ItemStocks.Add(st);
                            db.SaveChanges();
                        }
                        else
                        {
                            stock.Qty += Convert.ToInt32(itm.Qty);
                            db.SaveChanges();
                        }

                        //string itmname = db.Items.Where(a => a.ItemsId == itm.ItemsId).Select(b => b.Description).FirstOrDefault();
                        //desc += itmname + " " + itm.PurchaseRate + " x " + itm.Qty + " = " + (itm.PurchaseRate * itm.Qty) +"\n";
                    }

                    var create = await db.SaveChangesAsync();
                    if (create > 0)
                    {
                        addStatus = true;
                    }

                    //decimal? AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).Select(b => b.OpeningBalance).FirstOrDefault();

                    //StatementOfAccount stm = new StatementOfAccount();
                    //stm.date = (DateTime)ord.DatePurchase;
                    //stm.AccountCodeId = ord.AccountCodeId;
                    //stm.Description = desc;// "Purchase Note ";
                    //stm.ReportType = 2;
                    //stm.Rate = Convert.ToInt32(ord.GrandTotal);
                    //stm.Weight = weight;
                    //stm.Debit = Convert.ToInt32(ord.PayingAmount);
                    //stm.Credit = 0;// (ord.TotalExpenses > 0) ? Convert.ToInt32(ord.GrandTotal - ord.PayingAmount) : 0;
                    //stm.Labour = Convert.ToInt32(ord.TotalExpenses);
                    //stm.DocNo = ord.DocumentNumber.ToString();
                    //stm.Balance = Convert.ToInt32(AccOpnBal + Convert.ToInt32(ord.GrandTotal - ord.PayingAmount));// Convert.ToInt32(ord.GrandTotal - ord.PayingAmount);
                    //db.StatementOfAccounts.Add(stm);
                    //db.SaveChanges();

                    AccountCode accBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                    accBal.OpeningBalance = accBal.OpeningBalance - ord.TotalExpenses + (ord.GrandTotal - ord.PayingAmount);
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
       
        public bool VerifyExistPurchaseOrderItem(int purchaseOrderId, int productId) => db.PurchaseItems.Any(x => x.PurchaseGoodsOrderId == purchaseOrderId && x.ItemsId == productId);
        public async Task<List<PurchaseItem>> GetPurchaseItemsInfo(int ordId)
        {
            var item = await db.PurchaseItems.Where(a => a.PurchaseGoodsOrderId == ordId).ToListAsync();
            return item;
        }
        //========================== Report ========================
        public async Task<List<PurchaseNoteItems>> GetReportPurchaseItemsInfo(int ordId)
        {
            var item = await db.PurchaseItems.Where(a => a.PurchaseGoodsOrderId == ordId).Select(h => new PurchaseNoteItems { Code = h.Items.Code, Description = h.Items.Description, Qty = h.Qty, Notes = h.Notes, PurchasePrice = h.PurchaseRate }).ToListAsync();
            return item;
        }
        public async Task<List<PurchaseNoteDateWise>> GetDateWisePurchases(DateTime date)
        {
            int dayOpened = await db.dayOpening.Where(c => c.DateTimeOpening.Day == date.Day && c.DateTimeOpening.Month == date.Month && c.DateTimeOpening.Year == date.Year).Select(k => k.DayOpeningsId).SingleOrDefaultAsync();

            var item = await db.PurchaseGoods
                .Where(a => a.DayOpeningsId == dayOpened)
                .Select(a =>
                new PurchaseNoteDateWise
                {
                    AccountCode = a.AccountCode.Code,
                    AccountCodeName = a.AccountCode.Description,
                    DocumentNumber = a.DocumentNumber,
                    Notes = a.Notes,
                    GrandTotal = a.GrandTotal,
                    PayingAmount = a.PayingAmount,
                    Remaining = a.GrandTotal - a.PayingAmount
                }).ToListAsync();
            return item;
        }
    }
}
