using Application.Business.IAppServices;
using Application.Interface.Repositories;
using Domain.Entities;
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
    public class PartyCodeRepository : Repository<PartyCode>, IPartyCodeRepository
    {
        private readonly ERPDBContext db;

        public PartyCodeRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<bool> Update(PartyCode pCode)
        {
            try
            {
                bool updateStatus = false;
                
                var objFromDb = await db.PartyCodes.FirstOrDefaultAsync(c => c.PartyCodeId == pCode.PartyCodeId);
                objFromDb.Code = pCode.Code;
                objFromDb.Name = pCode.Name;
                objFromDb.Address = pCode.Address;
                objFromDb.city = pCode.city;
                objFromDb.phone = pCode.phone;
                objFromDb.fax = pCode.fax;
                objFromDb.AccountDate = pCode.AccountDate;
                objFromDb.contactPerson = pCode.contactPerson;
                objFromDb.creditDays = pCode.creditDays;
                objFromDb.NTN = pCode.NTN;
                objFromDb.STRN = pCode.STRN;
                objFromDb.remarks = pCode.remarks;
                objFromDb.MobileOne = pCode.MobileOne;
                objFromDb.MobileTwo = pCode.MobileTwo;
                //objFromDb.IsActive = pCode.IsActive;
                objFromDb.updated_at = DateTime.Now;
                var create = await db.SaveChangesAsync();
                if (create > 0)
                {
                    updateStatus = true;
                }
                _logger.LogInformation("Log message in the {Repo} method for update", typeof(PartyCodeRepository));
                return updateStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(PartyCodeRepository));
                return false;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetListPartyCodeForDropDown()
        {
            return await db.PartyCodes.Where(x => x.IsActive == true).Select(x => new SelectListItem()
            {
                Text = x.Code+" "+x.Name,
                Value = x.PartyCodeId.ToString()
            }).ToListAsync();
        }

        public async Task<bool> SetRecordAsDeleted(int Id)
        {
            int IsDeleted = 0;
            try
            {
                var objFromDb = await db.PartyCodes.FirstOrDefaultAsync(c => c.PartyCodeId == Id);
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
        public async Task<bool> GetExisting(string Name)
        {
            bool exist = false;
            var itemInfo = await db.PartyCodes.Where(a => a.Name == Name).FirstOrDefaultAsync();
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

        public async Task<PartyCodes> GetPartyCodeRepo(int AccountCodeId)
        {
            PartyCodes item = new PartyCodes();
            bool chk = await db.PartyCodes
                .Where(a => a.AccountCodeId == AccountCodeId).AnyAsync();
            if(chk == true)
            {
                item = await db.PartyCodes
                .Where(a => a.AccountCodeId == AccountCodeId)
                .Select(a => new PartyCodes
                {
                    PartyCodeId = a.PartyCodeId,
                    AccountCode = a.AccountCode.Code,
                    AccountCodeDescription = a.AccountCode.Description,
                    OpeningBalance = a.AccountCode.OpeningBalance,
                    AccountDate = a.AccountDate,
                    Address = a.Address,
                    city = a.city,
                    contactPerson = a.contactPerson,
                    creditDays = a.creditDays,
                    fax = a.fax,
                    Name = a.AccountCode.Description,
                    NTN = a.NTN,
                    phone = a.phone,
                    MobileOne = a.MobileOne,
                    MobileTwo = a.MobileTwo,
                    remarks = a.remarks,
                    STRN = a.STRN,
                    created_at = a.created_at
                })
                .FirstOrDefaultAsync();
            }
            else
            {
                item = await db.AccountCodes
                .Where(a => a.AccountCodeId == AccountCodeId)
                .Select(a => new PartyCodes
                {
                    PartyCodeId = a.AccountCodeId,
                    AccountCode = a.Code,
                    Name = a.Description,
                    OpeningBalance = a.OpeningBalance,
                    MobileOne = "-",
                    created_at = a.created_at
                })
                .FirstOrDefaultAsync();
            }
            return item;
        }

        public async Task<List<PartyCodes>> GetPartyCodesList()
        {
            var item = await db.PartyCodes.Where(a => a.IsActive == true)
                .OrderByDescending(a => a.PartyCodeId)
                .Select(a => new PartyCodes
                {
                    PartyCodeId = a.PartyCodeId,
                    AccountCode = a.AccountCode.Code,
                    AccountCodeDescription = a.AccountCode.Description,
                    OpeningBalance = a.AccountCode.OpeningBalance,
                    AccountDate = a.AccountDate,
                    Address = a.Address,
                    city = a.city,
                    contactPerson = a.contactPerson,
                    creditDays = a.creditDays,
                    fax = a.fax,
                    Name = a.Name,
                    NTN = a.NTN,
                    phone = a.phone,
                    MobileOne = a.MobileOne,
                    MobileTwo = a.MobileTwo,
                    remarks = a.remarks,
                    STRN = a.STRN,
                    created_at = a.created_at
                }).ToListAsync();
            return item;
        }
    }
}
