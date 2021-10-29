using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel;
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
        Task<IEnumerable<SelectListItem>> GetListAccountCodeForDropDown();
        Task<bool> SaveCustomAcountCode(AccountCode AccCode);
        Task<bool> Update(AccountCode AccCode);
        Task<bool> SetRecordAsDeleted(int Id);
        Task<bool> GetExistingCode(string Code);
        Task<List<AccountCodes>> GetAllAccountCodesRepo();
    }
}
