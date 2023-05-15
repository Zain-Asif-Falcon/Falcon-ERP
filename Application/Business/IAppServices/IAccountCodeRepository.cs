using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.API;
using Domain.ViewModel.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Business.IAppServices
{
    public interface IAccountCodeRepository : IRepository<AccountCode>
    {
        Task<List<AccountCodeViewModel>> GetAllAccountCodes();
        Task<IEnumerable<SelectListItem>> AccountCodeDropDown();
        Task<IEnumerable<SelectListItem>> AccountCodeDropDownByName();
        Task<GenericRequestResponse> SaveCustomAcountCode(AccountCode AccCode);
     }
}
