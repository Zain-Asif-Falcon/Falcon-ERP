using Application.Business.IAppServices;
using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.Reports;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            var item = await db.AccountCodes.OrderByDescending(a => a.AccountCodeId).Where(b => b.IsActive == true).Select(a => new AccountCodeViewModel {  AccHead = a.AccountHead.Description , AccCode = new AccountCode { AccountCodeId = a.AccountCodeId,  Code = a.Code,  Description = a.Description,  OpeningBalance = a.OpeningBalance,  AccountHeadId = a.AccountHeadId , created_at = a.created_at } }).ToListAsync(); ;
            //var item = await db.ItemStocks.ToListAsync();
            return item;
        }
        public async Task<bool> SaveCustomAcountCode(AccountCode aCode)
        {
            try
            {
                AccountCode code = new AccountCode();

                bool updateStatus = false;
                int dayOpened = await db.dayOpening.Where(c => c.closeStatus == false).Select(k => k.DayOpeningsId).SingleOrDefaultAsync();
                if(dayOpened > 0)
                {
                    code.AccountHeadId = aCode.AccountHeadId;
                    code.Code = aCode.Code;
                    code.OpeningBalance = aCode.OpeningBalance;
                    code.Description = aCode.Description;
                    code.IsActive = true;
                    code.created_at = DateTime.Now;
                    Add(code);
                    var create = await db.SaveChangesAsync();

                    CashRecieveds csh = new CashRecieveds();
                    csh = db.Cashes.Where(a => a.documentDate.Value.Year == DateTime.Now.Year && a.documentDate.Value.Month == DateTime.Now.Month && a.documentDate.Value.Day == DateTime.Now.Day).FirstOrDefault();

                    if (csh == null)
                    {
                        int? docNum = db.Cashes.OrderByDescending(a => a.CashRecievedId).Select(k => k.DocumentNumber).FirstOrDefault();
                        docNum++;
                        CashRecieveds cash = new CashRecieveds();
                        cash.DayOpeningsId = dayOpened;
                        cash.Description = "Cash Reciept Register of " + DateTime.Now.ToString("dd MMM yyyy");
                        cash.documentDate = DateTime.Now;
                        cash.DocumentNumber = docNum;
                        cash.IsActive = true;
                        cash.MonthNumber = DateTime.Now.Month;
                        db.Cashes.Add(cash);
                        db.SaveChanges();

                        csh = db.Cashes.Where(a => a.documentDate.Value.Year == DateTime.Now.Year && a.documentDate.Value.Month == DateTime.Now.Month && a.documentDate.Value.Day == DateTime.Now.Day).FirstOrDefault();

                        CashRecievedItems cshItm = new CashRecievedItems();
                        cshItm.CashRecievedId = csh.CashRecievedId;
                        cshItm.credit = aCode.OpeningBalance;
                        cshItm.debit = 0;
                        cshItm.AccountCodeId = code.AccountCodeId;
                        cshItm.Description = "Initial Opening Balance";
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
                        cshItm.credit = aCode.OpeningBalance;
                        cshItm.debit = 0;
                        cshItm.AccountCodeId = code.AccountCodeId;
                        cshItm.Description = "Initial Opening Balance";
                        cshItm.IsActive = true;
                        cshItm.created_at = DateTime.Now;
                        db.cashRecievingItems.Add(cshItm);
                        db.SaveChanges();
                    }

                    StatementOfAccount stm = new StatementOfAccount();
                    stm.date = DateTime.Now;
                    stm.AccountCodeId = code.AccountCodeId;
                    stm.Description = "Cash Recieved on Account Code Creation";
                    stm.Credit = Convert.ToInt32(aCode.OpeningBalance);
                    stm.Debit = 0;
                    stm.DocNo = "Cr #." + csh.DocumentNumber.ToString();
                    stm.OpeningBalance = Convert.ToInt32(aCode.OpeningBalance);
                    stm.Balance = Convert.ToInt32(aCode.OpeningBalance);
                    db.StatementOfAccounts.Add(stm);
                    db.SaveChanges();

                    if (create > 0)
                    {
                        updateStatus = true;
                    }
                }

                _logger.LogInformation("Log message in the {Repo} method for update", typeof(AccountCodeRepository));
                return updateStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(AccountCodeRepository));
                return false;
            }
        }
        public async Task<bool> Update(AccountCode aCode)
        {
            try
            {
                bool updateStatus = false;
                var objFromDb = await db.AccountCodes.FirstOrDefaultAsync(c => c.AccountCodeId == aCode.AccountCodeId);
                objFromDb.Code = aCode.Code;
                objFromDb.Description = aCode.Description;
                //objFromDb.AccountHeadId = aCode.AccountHeadId;
                objFromDb.OpeningBalance = aCode.OpeningBalance;
                //objFromDb.IsActive = aCode.IsActive;
                objFromDb.updated_at = DateTime.Now;
                var create = await db.SaveChangesAsync();
                if (create > 0)
                {
                    updateStatus = true;
                }
                _logger.LogInformation("Log message in the {Repo} method for update", typeof(AccountCodeRepository));
                return updateStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(AccountCodeRepository));
                return false;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetListAccountCodeForDropDown()
        {
            return await db.AccountCodes.Where(x => x.IsActive == true).Select(x => new SelectListItem()
            {
                Text = x.Code+"-"+x.Description,
                Value = x.AccountCodeId.ToString()
            }).ToListAsync();
        }
        
        public async Task<bool> SetRecordAsDeleted(int Id)
        {
            int IsDeleted = 0;
            try
            {
                var objFromDb = await db.AccountCodes.FirstOrDefaultAsync(c => c.AccountCodeId == Id);
                if (objFromDb != null)
                {
                    objFromDb.deleted_at = DateTime.Now;
                    objFromDb.IsActive = false;
                    IsDeleted = db.SaveChanges();
                }
                _logger.LogInformation("Log message in the {Repo} method", typeof(AccountCodeRepository));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(AccountCodeRepository));
                return false;
            }

            return (IsDeleted > 0) ? true : false;
        }
        public async Task<bool> GetExistingCode(string Code)
        {
            bool exist = false;
            var itemInfo = await db.AccountCodes.Where(a => a.Code == Code).FirstOrDefaultAsync();
            if (itemInfo == null)
            {
                exist = false;
            }
            else
            {
                exist = true;
            }
            return exist;
        }
        //================================================================
        public async Task<List<AccountCodes>> GetAllAccountCodesRepo()
        {
            var item = await db.AccountCodes.Select(a => new AccountCodes { Code = a.Code , Description = a.Description , OpeningBalance = a.OpeningBalance, AccountHeadCode = a.AccountHead.Code , AccountHeadDescription = a.AccountHead.Description}).ToListAsync(); ;
            //var item = await db.ItemStocks.ToListAsync();
            return item;
        }
    }
}
