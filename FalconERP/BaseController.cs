using Domain.Contracts.V1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystem.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public BaseController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        //public async Task<UserContract> getLoggedInUser()
        //{
        //    UserContract user = new UserContract();
        //    var loggedInUser = await _userManager.GetUserAsync(User);
        //    user.Id = loggedInUser.Id;
        //    user.Email = loggedInUser.Email;
        //    return user;
        //}
    }
}
