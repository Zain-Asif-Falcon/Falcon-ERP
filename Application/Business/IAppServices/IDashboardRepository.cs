using Domain.ViewModel.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.IAppServices
{
    public interface IDashboardRepository
    {
        Task<TicketsViewModel> GetTicketsCounts();
        Task<LatestFiveRecordsViewModel> GetLastFiveList();
        Task<MonthlyTotals> GetMonthlyTotals(DateTime dat);
        
    }
}
