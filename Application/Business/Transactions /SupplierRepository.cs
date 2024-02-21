using Application.Business.Transactions.IAppServices;
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

namespace Application.Business.Transactions
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private readonly ERPDBContext db;

        public SupplierRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<bool> Update(Supplier aCode)
        {
            try
            {
                bool updateStatus = false;
                var objFromDb = await db.Suppliers.FirstOrDefaultAsync(c => c.SuppliersId == aCode.SuppliersId);
                objFromDb.Address = aCode.Address;
                objFromDb.Email = aCode.Email;
                objFromDb.MobileNumber = aCode.MobileNumber;           
                objFromDb.updated_at = DateTime.Now;
                var create = await db.SaveChangesAsync();
                if (create > 0)
                {
                    updateStatus = true;
                }
                _logger.LogInformation("Log message in the {Repo} method for update", typeof(SupplierRepository));
                return updateStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(SupplierRepository));
                return false;
            }
        }
        
    }
}
