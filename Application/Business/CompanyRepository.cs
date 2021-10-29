using Application.Business.IAppServices;
using Application.Interface.Repositories;
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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ERPDBContext db;

        public CompanyRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<bool> Update(Company comp)
        {
            try
            {
                bool updateStatus = false;

                var objFromDb = await db.Companies.FirstOrDefaultAsync(c => c.CompanyId == comp.CompanyId);
                objFromDb.Name = comp.Name;
                objFromDb.Address = comp.Address;
                objFromDb.phone = comp.phone;
                objFromDb.NTN = comp.NTN;
                objFromDb.STRN = comp.STRN;
                //objFromDb.IsActive = comp.IsActive;
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

        public IEnumerable<SelectListItem> GetListCompanyForDropDown()
        {
            return db.Companies.Where(x => x.IsActive == true).Select(x => new SelectListItem()
            {
                Text = x.CompanyId+"-"+x.Name,
                Value = x.CompanyId.ToString()
            });
        }

        public async Task<bool> SetRecordAsDeleted(int Id)
        {
            int IsDeleted = 0;
            try
            {
                var objFromDb = await db.Companies.FirstOrDefaultAsync(c => c.CompanyId == Id);
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
            var itemInfo = await db.Companies.Where(a => a.Name == Name).FirstOrDefaultAsync();
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
