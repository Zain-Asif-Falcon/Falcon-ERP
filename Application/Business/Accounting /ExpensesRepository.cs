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
    public class ExpensesRepository : Repository<Expenses>, IExpensesRepository
    {
        private readonly ERPDBContext db;

        public ExpensesRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public async Task<List<ExpensesViewModel>> GetAllExpenses()
        {
            var item = await db.Expense.OrderByDescending(b => b.ExpensesId).Select(a => new ExpensesViewModel {  monthopening = a.MonthOpening.Name,  expensehead = a.ExpenseHeads.Name , expenses = new Expenses { ExpensesId = a.ExpensesId,Name = a.Name, Amount = a.Amount, date = a.date,Notes = a.Notes } }).ToListAsync(); ;
            return item;
        }
        public async Task<bool> SaveExpense(Expenses aCode)
        {
            try
            {
                bool updateStatus = false;

                MonthOpenings monthOpened = await db.MonthOpening.Where(c => c.closeStatus == false).SingleOrDefaultAsync();
                int dayOpened = await db.dayOpening.Where(c => c.closeStatus == false).Select(k => k.DayOpeningsId).SingleOrDefaultAsync();
                if (dayOpened > 0)
                {
                    Expenses exp = new Expenses();
                    exp.Name = aCode.Name;
                    exp.Amount = aCode.Amount;
                    exp.date = aCode.date;
                    exp.DayOpeningsId = dayOpened;
                    exp.MonthOpeningsId = monthOpened.MonthOpeningsId;
                    exp.Notes = aCode.Notes;
                    exp.date = aCode.date;
                    exp.IsActive = aCode.IsActive;
                    exp.ExpenseHeadsId = aCode.ExpenseHeadsId;
                    exp.updated_at = DateTime.Now;
                    Add(exp);
                }

                var create = await db.SaveChangesAsync();
                if (create > 0)
                {
                    updateStatus = true;
                    
                    monthOpened.OpeningBalance -= aCode.Amount;
                    db.SaveChanges();
                }
                _logger.LogInformation("Log message in the {Repo} method for update", typeof(ExpensesRepository));
                return updateStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(ExpensesRepository));
                return false;
            }
        }
        
        public IEnumerable<SelectListItem> GetListExpensesForDropDown()
        {
            return db.Expense.Where(x => x.IsActive == true).Select(x => new SelectListItem()
            {
                Text = x.ExpensesId + "-" + x.Amount.ToString(),
                Value = x.ExpensesId.ToString()
            });
        }
    }
}
