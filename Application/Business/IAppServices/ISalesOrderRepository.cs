using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel.Reports;
using Domain.ViewModel.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Transactions.IAppServices
{
    public interface ISalesOrderRepository : IRepository<SalesGoods>
    {
        Task<List<SaleNoteListVM>> GetSaleOrders();
        Task<bool> PlaceSaleOrder(SalesOrderViewModel purOrder);
    }
}