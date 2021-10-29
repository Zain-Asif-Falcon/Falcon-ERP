using Application.Interface.Repositories;
using System;
using System.Collections.Generic;
using Domain.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.ViewModel.API;
using Domain.Contracts.V1;
using Domain.ViewModel;

namespace Application.Business.IAppServices
{
    public interface IItemsRepository : IRepository<Item>
    {
        Task<ItemsViewModel> GetItemInfo(int id);
        Task<IEnumerable<SelectListItem>> GetListItemsForDropDown();
        Task<bool> Update(Item itm);
        Task<bool> SetRecordAsDeleted(int Id);
        Task<bool> GetExixtingCode(string Code);
    }
}
