using Application.Interface.Repositories;
using Domain.Entities;
using Domain.ViewModel.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Business.IAppServices
{
    public interface IPartyCodeRepository : IRepository<PartyCode>
    {
        Task<List<PartyCodes>> GetPartyCodesList();
        Task<IEnumerable<SelectListItem>> GetListPartyCodeForDropDown();
        Task<bool> Update(PartyCode pCode);
        Task<bool> SetRecordAsDeleted(int Id);
        Task<bool> GetExisting(string Name);
        Task<PartyCodes> GetPartyCodeRepo(int AccountCodeId);
        
    }
}
