using Application.Interface.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Business.IAppServices
{
    public interface ICompanyRepository : IRepository<Company>
    {
        IEnumerable<SelectListItem> GetListCompanyForDropDown();
        Task<bool> Update(Company comp);
        Task<bool> SetRecordAsDeleted(int Id);
        Task<bool> GetExisting(string Name);
    }
}
