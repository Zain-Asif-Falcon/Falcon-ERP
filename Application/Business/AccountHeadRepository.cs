using Application.Business.IAppServices;
using Application.Interface.Repositories;
using Domain.Contracts.V1;
using Domain.Entities;
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
    class AccountHeadRepository : Repository<AccountHead>, IAccountHeadRepository
    {
        private readonly ERPDBContext db;

        public AccountHeadRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<bool> Update(AccountHead itm)
        {
            bool updateStatus = false;
            var objFromDb = await db.AccountHeads.FirstOrDefaultAsync(c => c.AccountHeadId == itm.AccountHeadId);
            objFromDb.Code = itm.Code;
            objFromDb.Description = itm.Description;
            objFromDb.updated_at = DateTime.Now;
            var create = db.SaveChanges();
            if (create > 0)
            {
                updateStatus = true;
            }
            return updateStatus;
        }

        public async Task<IEnumerable<SelectListItem>> GetListAccountHeadForDropDown()
        {
            return await db.AccountHeads.Where(x => x.IsActive == true)
                .OrderBy(p => p.Description)
                .Select(x => new SelectListItem()
                        {
                            Text = x.AccountHeadId+"-"+ x.Description,
                            Value = x.AccountHeadId.ToString()
                        }).ToListAsync();
        }
        
        public async Task<bool> SetRecordAsDeleted(int Id)
        {
            int IsDeleted = 0;
            var objFromDb = await db.AccountHeads.FirstOrDefaultAsync(c => c.AccountHeadId == Id);
            if (objFromDb != null)
            {
                objFromDb.deleted_at = DateTime.Now;
                objFromDb.IsActive = false;
                IsDeleted = db.SaveChanges();
            }
            return (IsDeleted > 0) ? true : false;
        }
        public async Task<bool> GetExistingCode(string Code)
        {
            bool exist = false;
            var itemInfo = await db.AccountHeads.Where(a => a.Code == Code).FirstOrDefaultAsync();
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
    }
}
