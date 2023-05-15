using Application.Business.IAppServices;
using Application.Common.Utilities;
using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.API;
using Domain.ViewModel.Reports;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Business
{
    public class AccountCodeRepository : Repository<AccountCode>, IAccountCodeRepository
    {
        private readonly ERPDBContext db;

        public AccountCodeRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<List<AccountCodeViewModel>> GetAllAccountCodes()
        {
            var item = await db.AccountCodes
                .OrderByDescending(a => a.AccountCodeId)
                .Where(b => b.IsActive == true)
                .Select(a => new AccountCodeViewModel
                {
                    AccHead = a.AccountHead.Description,
                    AccCode = new AccountCode
                    {
                        AccountCodeId = a.AccountCodeId,
                        Code = a.Code,
                        Description = a.Description,
                        FixedOpeningBalance = a.FixedOpeningBalance,
                        OpeningBalance = a.OpeningBalance,
                        AccountHeadId = a.AccountHeadId,
                        created_at = a.created_at
                    }
                }).ToListAsync(); ;
            //var item = await db.ItemStocks.ToListAsync();
            return item;
        }
        public async Task<GenericRequestResponse> SaveCustomAcountCode(AccountCode aCode)
        {
            try
            {
                bool succStatus = false;
                string msg = "No Change";
                AccountCode code = new AccountCode();

                bool updateStatus = false;

                var newCode = await db.AccountCodes.AnyAsync(c => c.Code == aCode.Code && c.IsActive == true);
                if (newCode == false)
                {
                    int dayOpened = await db.dayOpening.Where(c => c.closeStatus == false).Select(k => k.DayOpeningsId).SingleOrDefaultAsync();
                    var newName = await db.AccountCodes.AnyAsync(c => c.Description == aCode.Description && c.IsActive == true);
                    if (newName == false)
                    {
                        if (dayOpened > 0)
                        {
                            string headDt = db.AccountHeads.Where(a => a.AccountHeadId == aCode.AccountHeadId).Select(m => m.Code).FirstOrDefault();

                            code.AccountHeadId = aCode.AccountHeadId;
                            code.Code = aCode.Code;
                            code.OpeningBalance = aCode.FixedOpeningBalance;
                            code.FixedOpeningBalance = aCode.FixedOpeningBalance;
                            code.Description = Helper.ToTitleCase(aCode.Description);
                            code.IsActive = true;
                            code.created_at = DateTime.Now;
                            Add(code);
                            var create = await db.SaveChangesAsync();
                            CashRecieveds csh = new CashRecieveds();
                            csh = db.Cashes.Where(a => a.documentDate.Value.Year == DateTime.Now.Year && a.documentDate.Value.Month == DateTime.Now.Month && a.documentDate.Value.Day == DateTime.Now.Day).FirstOrDefault();

                            if (csh == null)
                            {
                                int? docNum = db.Cashes.OrderByDescending(a => a.CashRecievedId).Select(k => k.DocumentNumber).FirstOrDefault();
                                if (docNum == null)
                                {
                                    string dcNum = DateTime.Now.ToString("yy") + String.Format("{0:D2}", DateTime.Now.Month) + 1.ToString("D4");
                                    docNum = int.Parse(dcNum);
                                }
                                else
                                {
                                    docNum++;
                                }
                                CashRecieveds cash = new CashRecieveds();
                                cash.DayOpeningsId = dayOpened;
                                cash.Description = "Cash Receipt Register of " + DateTime.Now.ToString("dd MMM yyyy");
                                cash.documentDate = DateTime.Now;
                                cash.DocumentNumber = docNum;
                                //add financial year by AD
                                cash.FiscalYearId = 1;
                                cash.IsActive = true;
                                cash.MonthNumber = DateTime.Now.Month;
                                cash.created_at = DateTime.Now;
                                db.Cashes.Add(cash);
                                db.SaveChanges();

                                csh = db.Cashes.Where(a => a.documentDate.Value.Year == DateTime.Now.Year && a.documentDate.Value.Month == DateTime.Now.Month && a.documentDate.Value.Day == DateTime.Now.Day).FirstOrDefault();

                                CashRecievedItems cshItm = new CashRecievedItems();
                                cshItm.CashRecievedId = csh.CashRecievedId;
                                cshItm.credit = code.OpeningBalance;
                                cshItm.debit = 0;
                                cshItm.AccountCodeId = code.AccountCodeId;
                                cshItm.Description = "Opening Balance";
                                cshItm.IsActive = true;
                                cshItm.created_at = DateTime.Now;
                                db.cashRecievingItems.Add(cshItm);
                                db.SaveChanges();
                            }
                            else
                            {
                                csh = db.Cashes.Where(a => a.documentDate.Value.Year == DateTime.Now.Year && a.documentDate.Value.Month == DateTime.Now.Month && a.documentDate.Value.Day == DateTime.Now.Day).FirstOrDefault();

                                CashRecievedItems cshItm = new CashRecievedItems();
                                cshItm.CashRecievedId = csh.CashRecievedId;
                                cshItm.credit = code.OpeningBalance;
                                cshItm.debit = 0;
                                cshItm.AccountCodeId = code.AccountCodeId;
                                cshItm.Description = "Opening Balance";
                                cshItm.IsActive = true;
                                cshItm.created_at = DateTime.Now;
                                db.cashRecievingItems.Add(cshItm);
                                db.SaveChanges();
                            }

                            //StatementOfAccount stm = new StatementOfAccount();
                            //stm.date = DateTime.Now;
                            //stm.AccountCodeId = code.AccountCodeId;
                            //stm.Description = "Cash Recieved on Account Code Creation";
                            //stm.Credit = Convert.ToInt32(aCode.OpeningBalance);
                            //stm.Debit = 0;
                            //stm.DocNo = "Cr #." + csh.DocumentNumber.ToString();
                            //stm.OpeningBalance = Convert.ToInt32(aCode.OpeningBalance);
                            //stm.Balance = Convert.ToInt32(aCode.OpeningBalance);
                            //db.StatementOfAccounts.Add(stm);
                            //db.SaveChanges();

                            if (create > 0)
                            {
                                msg = "Account Code has been saved";
                                succStatus = true;
                            }
                        }
                    }
                    else
                    {
                        msg = "Account Code name already exist";
                        succStatus = false;
                    }
                }
                else
                {
                    msg = "Account Code  already exist";
                    succStatus = false;
                }
              
                _logger.LogInformation("Log message in the {Repo} method for update", typeof(AccountCodeRepository));
                //return updateStatus;
                return new GenericRequestResponse()
                {
                    Success = succStatus,
                    Message = msg
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(AccountCodeRepository));
                return new GenericRequestResponse()
                {
                    Success = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
