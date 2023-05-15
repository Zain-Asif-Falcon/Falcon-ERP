using Application.Business.Transactions.IAppServices;
using Application.Common.Utilities;
using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel.API;
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
        public async Task<List<SaleNoteListVM>> GetSaleOrders(int fiscalYearId)
        {
            var item = await db.SalesGood
                .OrderByDescending(a => a.SalesGoodsId)
                .Where(a => a.AccountCode.IsActive == true && a.FiscalYearId == fiscalYearId)
                .Select(b => new SaleNoteListVM
                {
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

        public async Task<SalesGoods> GetSalesGoodInfo(int ordId)
        {
            SalesGoods itm = new SalesGoods();
            if (ordId > 0)
            {
                itm = await db.SalesGood.Where(a => a.SalesGoodsId == ordId).FirstOrDefaultAsync();
            }
            if (ordId == 0)
            {
                itm = await db.SalesGood.OrderByDescending(a => a.SalesGoodsId).FirstOrDefaultAsync();
            }
            return itm;
        }
        public async Task<GenericRequestResponse> PlaceSaleOrder(SalesOrderViewModel purOrder)
        {
            //int? weight = 0;
            //decimal? price = 0;
            //decimal? totSal = 0;

            //string desc = "<h5>Sales Note </h5> <table class='table table-striped table-responsive table-bordered'style='font-size:12px'><thead><tr><th>Item</th><th>Rate</th><th>Weight</th><th>Total</th></tr></thead><tbody>";
            bool succStatus = false;
            string msg = "Something went wrong";
            var create = 0;
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
                    //ord.Notes = Helper.ToTitleCase(purOrder.order.Notes);
                    ord.ReferenceNum = purOrder.order.ReferenceNum;
                    ord.AccountCodeId = purOrder.order.AccountCodeId;
                    ord.Acknowledged = false;
                    ord.FiscalYearId = purOrder.order.FiscalYearId;
                    ord.DueDate = purOrder.order.DueDate;
                    ord.DateTimeEntered = DateTime.Now;
                    Add(ord);
                    create = await db.SaveChangesAsync();// db.SaveChanges();
                    if (create > 0)
                    {
                        succStatus = true;
                        msg = "Data has been saved";
                        //addStatus = true;
                    }
                    foreach (var itm in purOrder.items)
                    {
                        SalesItems purItems = new SalesItems();
                        purItems.ItemsId = itm.ItemsId;
                        purItems.SalesGoodsOrderId = ord.SalesGoodsId;
                        purItems.Qty = itm.Qty;
                        purItems.SalePrice = itm.SalePrice;
                        purItems.Notes = (itm.Notes != null) ? Helper.ToTitleCase(itm.Notes) : "";
                        purItems.DateTimeEntered = DateTime.Now;
                        db.SalesItem.Add(purItems);
                        db.SaveChanges();

                        //weight += purItems.Qty;
                        //price += purItems.SalePrice;
                        //totSal += purItems.Qty * purItems.SalePrice;

                        Item st = db.Items.Where(a => a.ItemsId == itm.ItemsId).FirstOrDefault();
                        st.StockQty -= Convert.ToInt32(itm.Qty);
                        db.SaveChanges();

                        //string itmname = db.Items.Where(a => a.ItemsId == itm.ItemsId).Select(b => b.Description).FirstOrDefault();
                        //desc += "<tr><td>"+ itmname + "</td><td>"+ itm.SalePrice + "</td><td>" + itm.Qty + "</td><td>" + (itm.SalePrice * itm.Qty) + "</td></tr>";
                    }

                    //desc += "</tbody><tfoot><tr><th>Total:</th><th>"+price+"</th><th>" + weight +"</th><th>"+ totSal + "</th></tr></tfoot></table>";

                    create = await db.SaveChangesAsync();
                    if (create > 0)
                    {
                        succStatus = true;
                        msg = "Data has been saved";
                        //addStatus = true;
                    }

                    //decimal? AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).Select(b => b.OpeningBalance).FirstOrDefault();


                    AccountCode accBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                    accBal.OpeningBalance = accBal.OpeningBalance - ord.TotalExpenses - (ord.InvoiceAmount - ord.PayingAmount);
                    db.SaveChanges();

                    dbContextTransaction.Commit();

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                   

                }
                //return addStatus;
                return new GenericRequestResponse()
                {
                    Success = succStatus,
                    Message = msg
                };
            }
        }
        public async Task<GenericRequestResponse> EditPlaceSaleOrder(SalesOrderViewModel purOrder)
        {
            int? weight = 0;
            bool succStatus = false;
            string msg = "Something went wrong";
            var create = 0;


            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    decimal? oldrecievingAmount = 0;
                    decimal? oldGrandTotalAmount = 0;
                    int? oldAccountCodeId = 0;

                    SalesGoods ord = await db.SalesGood.Where(a => a.SalesGoodsId == purOrder.order.SalesGoodsId).FirstOrDefaultAsync();

                    oldrecievingAmount = ord.PayingAmount;
                    oldGrandTotalAmount = ord.GrandTotal;
                    //Add by AD
                    oldAccountCodeId = ord.AccountCodeId;
                    ord.AccountCodeId = purOrder.order.AccountCodeId;
                    //End 
                    ord.Carriage = purOrder.order.Carriage;
                    ord.TotalExpenses = purOrder.order.TotalExpenses;
                    ord.Labour = purOrder.order.Labour;
                    ord.DocumentNumber = purOrder.order.DocumentNumber;
                    ord.Cutting = purOrder.order.Cutting;
                    ord.Carrogation = purOrder.order.Carrogation;
                    ord.Misc = purOrder.order.Misc;
                    ord.Loading = purOrder.order.Loading;
                    ord.GrandTotal = purOrder.order.GrandTotal;
                    ord.InvoiceAmount = purOrder.order.InvoiceAmount;
                    //ord.Notes = Helper.ToTitleCase(purOrder.order.Notes);
                    ord.ReferenceNum = purOrder.order.ReferenceNum;
                    ord.PayingAmount = purOrder.order.PayingAmount;
                    ord.DueDate = purOrder.order.DueDate;
                    ord.FiscalYearId = purOrder.order.FiscalYearId;
                    Update(ord);
                    //await db.SaveChangesAsync();
                    create = await db.SaveChangesAsync();
                    if (create > 0)
                    {
                        succStatus = true;
                        msg = "Data has been saved";
                    }

                    foreach (var itm in purOrder.items)
                    {
                        if (VerifyExistSaleOrderItem((int)purOrder.order.SalesGoodsId, (int)itm.ItemsId))
                        {
                            decimal? oldItmQty = 0;

                            SalesItems editItems = await db.SalesItem.Where(s => s.ItemsId == itm.ItemsId && s.SalesGoodsOrderId == purOrder.order.SalesGoodsId).FirstOrDefaultAsync();

                            oldItmQty = editItems.Qty;

                            editItems.SalePrice = itm.SalePrice;
                            editItems.Qty = itm.Qty;
                            editItems.Notes = (itm.Notes != null) ? Helper.ToTitleCase(itm.Notes) : "";
                            db.SalesItem.Update(editItems);
                            db.SaveChanges();

                            if (itm.Qty > oldItmQty)
                            {
                                Item st = db.Items.Where(a => a.ItemsId == itm.ItemsId).FirstOrDefault();
                                st.StockQty = Convert.ToInt32(itm.StockQty - itm.Qty);
                                db.SaveChanges();

                                //AccountCode accBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                                //accBal.OpeningBalance = accBal.OpeningBalance - (itm.SalePrice * itm.Qty);
                                //db.SaveChanges();
                            }
                            if (itm.Qty < oldItmQty)
                            {
                                Item st = db.Items.Where(a => a.ItemsId == itm.ItemsId).FirstOrDefault();
                                //st.StockQty = Convert.ToInt32(st.StockQty +(itm.StockQty - st.StockQty)- itm.Qty);
                                st.StockQty = Convert.ToInt32(itm.StockQty  - itm.Qty);
                                db.SaveChanges();

                                //AccountCode accBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                                //accBal.OpeningBalance = accBal.OpeningBalance + (itm.SalePrice * itm.Qty);
                                //db.SaveChanges();
                            }

                            //weight += editItems.Qty;
                        }
                        else
                        {
                            SalesItems purItems = new SalesItems();
                            purItems.ItemsId = itm.ItemsId;
                            purItems.SalesGoodsOrderId = ord.SalesGoodsId;
                            purItems.SalePrice = itm.SalePrice;
                            purItems.Notes = (itm.Notes != null) ? Helper.ToTitleCase(itm.Notes) : "";
                            purItems.Qty = (int)itm.Qty;
                            purItems.DateTimeEntered = DateTime.Now;
                            db.SalesItem.Add(purItems);
                            db.SaveChanges();

                            Item st = db.Items.Where(a => a.ItemsId == itm.ItemsId).FirstOrDefault();
                            st.StockQty -= Convert.ToInt32(itm.Qty);
                            db.SaveChanges();

                            //AccountCode accBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                            //accBal.OpeningBalance -= (itm.SalePrice * itm.Qty);
                            //db.SaveChanges();
                            //weight += purItems.Qty;
                        }
                    }
                    ///No need this below code to next comment
                    if (oldrecievingAmount != purOrder.order.PayingAmount)
                    {
                        if (purOrder.order.PayingAmount > oldrecievingAmount)
                        {
                            AccountCode AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                            AccOpnBal.OpeningBalance += purOrder.order.PayingAmount - oldrecievingAmount;
                            db.SaveChanges();
                        }
                        if (purOrder.order.PayingAmount < oldrecievingAmount)
                        {
                            AccountCode AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                            AccOpnBal.OpeningBalance -= (AccOpnBal.OpeningBalance < 0) ? (-(purOrder.order.PayingAmount - oldrecievingAmount)) : (purOrder.order.PayingAmount - oldrecievingAmount);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        if (purOrder.order.GrandTotal != oldGrandTotalAmount)
                        {
                            if (purOrder.order.PayingAmount == oldrecievingAmount)
                            {
                                //AccountCode AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                                ////AccOpnBal.OpeningBalance -= (AccOpnBal.OpeningBalance < 0) ? (-(purOrder.order.GrandTotal - oldGrandTotalAmount)) : (purOrder.order.GrandTotal - oldGrandTotalAmount);
                                //AccOpnBal.OpeningBalance = (AccOpnBal.OpeningBalance <= -1) ? AccOpnBal.OpeningBalance - (purOrder.order.GrandTotal - oldGrandTotalAmount) : AccOpnBal.OpeningBalance - (purOrder.order.GrandTotal - oldGrandTotalAmount);

                                //db.SaveChanges();
                            }
                            if (purOrder.order.PayingAmount > oldrecievingAmount && purOrder.order.GrandTotal != purOrder.order.PayingAmount)
                            {
                                AccountCode AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                                AccOpnBal.OpeningBalance -= (AccOpnBal.OpeningBalance < 0) ? (-(purOrder.order.GrandTotal - oldGrandTotalAmount)) : (purOrder.order.GrandTotal - oldGrandTotalAmount);
                                db.SaveChanges();
                            }
                        }
                    }
                    ///No need above code
                    if (oldAccountCodeId != purOrder.order.AccountCodeId)
                    {
                        if(purOrder.order.GrandTotal == oldGrandTotalAmount)
                        {
                            AccountCode AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == oldAccountCodeId).FirstOrDefault();
                            AccOpnBal.OpeningBalance = (AccOpnBal.OpeningBalance < 0) ? AccOpnBal.OpeningBalance + (purOrder.order.GrandTotal) : AccOpnBal.OpeningBalance + purOrder.order.GrandTotal;


                            AccountCode accOpnBalNewAccount = db.AccountCodes.Where(a => a.AccountCodeId == purOrder.order.AccountCodeId).FirstOrDefault();
                            accOpnBalNewAccount.OpeningBalance = (accOpnBalNewAccount.OpeningBalance < 0) ? accOpnBalNewAccount.OpeningBalance - (purOrder.order.GrandTotal) : accOpnBalNewAccount.OpeningBalance - purOrder.order.GrandTotal;
                            //accOpnBalNewAccount.OpeningBalance = (accOpnBalNewAccount.OpeningBalance < 0) ? AccOpnBal.OpeningBalance + (purOrder.order.GrandTotal) : AccOpnBal.OpeningBalance - purOrder.order.GrandTotal;

                        }
                        else
                        {
                            AccountCode AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == oldAccountCodeId).FirstOrDefault();
                            AccOpnBal.OpeningBalance = (AccOpnBal.OpeningBalance < 0) ? AccOpnBal.OpeningBalance + (oldGrandTotalAmount) : AccOpnBal.OpeningBalance - oldGrandTotalAmount;


                            AccountCode accOpnBalNewAccount = db.AccountCodes.Where(a => a.AccountCodeId == purOrder.order.AccountCodeId).FirstOrDefault();
                            accOpnBalNewAccount.OpeningBalance = (accOpnBalNewAccount.OpeningBalance < 0) ? accOpnBalNewAccount.OpeningBalance - (purOrder.order.GrandTotal) : accOpnBalNewAccount.OpeningBalance - purOrder.order.GrandTotal;
                        }



                        //accOpnBalNewAccount.OpeningBalance = accOpnBalNewAccount.OpeningBalance - ord.TotalExpenses - (ord.InvoiceAmount - ord.PayingAmount);

                    }
                    else
                    {
                        AccountCode AccOpnBal = db.AccountCodes.Where(a => a.AccountCodeId == ord.AccountCodeId).FirstOrDefault();
                        //AccOpnBal.OpeningBalance -= (AccOpnBal.OpeningBalance < 0) ? (-(purOrder.order.GrandTotal - oldGrandTotalAmount)) : (purOrder.order.GrandTotal - oldGrandTotalAmount);
                        AccOpnBal.OpeningBalance = (AccOpnBal.OpeningBalance <= -1) ? AccOpnBal.OpeningBalance - (purOrder.order.GrandTotal - oldGrandTotalAmount) : AccOpnBal.OpeningBalance - (purOrder.order.GrandTotal - oldGrandTotalAmount);

                        db.SaveChanges();
                    }

                    create = await db.SaveChangesAsync();
                    if (create > 0)
                    {
                        succStatus = true;
                        msg = "Data has been saved";
                    }

                    dbContextTransaction.Commit();

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
                //return addStatus;
                return new GenericRequestResponse()
                {
                    Success = succStatus,
                    Message = msg
                };
            }
        }    
    }
}
