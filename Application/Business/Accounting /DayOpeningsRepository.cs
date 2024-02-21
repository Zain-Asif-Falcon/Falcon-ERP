using Application.Business.Accounting.IAppServices;
using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel.Accounts;
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

namespace Application.Business.Accounting
{
    public class DayOpeningsRepository : Repository<DayOpenings>, IDayOpeningsRepository
    {
        private readonly ERPDBContext db;

        public DayOpeningsRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        
        public async Task<bool> CreateCustom(DayOpenings aCode)
        {
            try
            {
                bool updateStatus = false;
                //int dayOpened = await db.dayOpening.Where(c => c.closeStatus == false).Select(k => k.DayOpeningsId).SingleOrDefaultAsync();
                int monthOpened = await db.MonthOpening.Where(c => c.closeStatus == false).Select(k => k.MonthOpeningsId).SingleOrDefaultAsync();
                var dayOpened = await db.dayOpening.Where(c => c.closeStatus == false).FirstOrDefaultAsync();
                if (dayOpened == null && monthOpened > 0)
                {
                    DayOpenings day = new DayOpenings(); //await db.MonthOpening.FirstOrDefaultAsync(c => c.MonthOpeningsId == aCode.MonthOpeningsId);
                    day.DateTimeOpening = aCode.DateTimeOpening;
                    day.OpeningBalance = aCode.OpeningBalance;
                    day.MonthOpeningsId = monthOpened;
                    day.closeStatus = false;
                    //mon.Notes = aCode.Notes;
                    Add(day);
                    var create = await db.SaveChangesAsync();
                    if (create > 0)
                    {
                        updateStatus = true;
                    }
                    _logger.LogInformation("Log message in the {Repo} method for update", typeof(DayOpeningsRepository));
                }
                else
                {
                    _logger.LogInformation("Log message in the {Repo} method for already Day Openings Found", typeof(DayOpeningsRepository));
                }
                return updateStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(DayOpeningsRepository));
                return false;
            }
        }

        public async Task<NewOpeningBalanceVM> GetOpeningBalance()
        {
            NewOpeningBalanceVM result = new NewOpeningBalanceVM
            {
                openingBalance = await db.dayOpening.OrderByDescending(a => a.DayOpeningsId).Select(a => a.ClosingBalance).FirstOrDefaultAsync()
            };
            return result; 
        }
  
    }
}
