using Application.Interface.Repositories;
using Domain.Contracts.V1;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Business.IAppServices
{
    public interface IAccountHeadRepository : IRepository<AccountHead>
    {    
        Task<IEnumerable<SelectListItem>> GetListAccountHeadForDropDown();  
        Task<GenericRequestResponse> Update(AccountHead actionOwner);
        Task<bool> SetRecordAsDeleted(int Id);
        Task<bool> GetExistingName(string Name);
    }
}
