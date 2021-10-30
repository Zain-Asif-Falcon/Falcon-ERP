using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Business.Administration.IAppServices;
using Domain.ViewModel.Administration;
using ERPSystem.Extensions;
using ERPSystem.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ERPSystem.Controllers
{
	public class AdminController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ILogger<AdminController> _logger;
		private readonly IDataAccessService _dataAccessService;
		private readonly SignInManager<IdentityUser> _signInManager;
		public AdminController(
				UserManager<IdentityUser> userManager,
				RoleManager<IdentityRole> roleManager,
				IDataAccessService dataAccessService,
				SignInManager<IdentityUser> signInManager,
				ILogger<AdminController> logger)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_dataAccessService = dataAccessService;
			_logger = logger;
		}

		[Authorize]
		public async Task<IActionResult> Roles()
		{
			var roleViewModel = new List<RolesViewModel>();

			try
			{
				var roles = await _roleManager.Roles.ToListAsync();
				foreach (var item in roles)
				{
					roleViewModel.Add(new RolesViewModel()
					{
						Id = item.Id,
						RoleName = item.Name,
					});
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, ex.GetBaseException().Message);
			}

			return View(roleViewModel);
		}

		//[Authorize("Roles")]
		public IActionResult CreateRole()
		{
			return View(new RolesViewModel());
		}

		[HttpPost]
		//[Authorize("Roles")]
		public async Task<IActionResult> CreateRole(RolesViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var result = await _roleManager.CreateAsync(new IdentityRole() { Name = viewModel.RoleName });
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Roles));
				}
				else
				{
					ModelState.AddModelError("Name", string.Join(",", result.Errors));
				}
			}
			return View(viewModel);
		}

		//[Authorize("Authorization")]
		public async Task<IActionResult> Users()
		{
			var userViewModel = new List<UserViewModel>();

			try
			{
				var users = await _userManager.Users.ToListAsync();
				foreach (var item in users)
				{
					userViewModel.Add(new UserViewModel()
					{
						Id = item.Id,
						Email = item.Email,      // ============
						UserName = item.UserName,
					});
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, ex.GetBaseException().Message);
			}

			return View(userViewModel);
		}

		//[Authorize("Users")]
		public async Task<IActionResult> EditUser(string id)
		{
			var viewModel = new UserViewModel();
			if (!string.IsNullOrWhiteSpace(id))
			{
				var user = await _userManager.FindByIdAsync(id);
				var userRoles = await _userManager.GetRolesAsync(user);

				viewModel.Email = user?.Email;
				viewModel.UserName = user?.UserName;

				var allRoles = await _roleManager.Roles.ToListAsync();
				viewModel.Roles = allRoles.Select(x => new RolesViewModel()
				{
					Id = x.Id,
					RoleName = x.Name,
					Selected = userRoles.Contains(x.Name)
				}).ToArray();

			}

			return View(viewModel);
		}

		[HttpPost]
		//[Authorize("Users")]
		public async Task<IActionResult> EditUser(UserViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(viewModel.Id);
				var userRoles = await _userManager.GetRolesAsync(user);

				user.Email = viewModel.Email;
				user.UserName = viewModel.UserName;
				var result = await _userManager.UpdateAsync(user);

				await _userManager.RemoveFromRolesAsync(user, userRoles);
				await _userManager.AddToRolesAsync(user, viewModel.Roles.Where(x => x.Selected).Select(x => x.RoleName));

				return RedirectToAction(nameof(Users));
			}

			return View(viewModel);
		}

		//[Authorize("Authorization")]
		public async Task<IActionResult> EditRolePermission(string id)
		{
			HttpContext.Session.SetObject(Domain.Utility.SD.SessionRoleId, id);
			var permissions = new List<NavigationMenuViewModel>();
			if (!string.IsNullOrWhiteSpace(id))
			{
				permissions = await _dataAccessService.GetPermissionsByRoleIdAsync(id);

				List<TreeViewNode> nodes = new List<TreeViewNode>();
				foreach (var item in permissions)
                {
					nodes.Add(new TreeViewNode
					{
						id = item.Id,
						parent = item.ParentMenuId.ToString() == "" ? "#" : item.ParentMenuId.ToString(),
						text = item.Name.ToString(),
						state = new state {  opened = (item.Permitted == true)? true:false , selected = item.Permitted }
					});
				}
				ViewBag.Json = JsonConvert.SerializeObject(nodes);
			}

			return View(permissions);
		}

		[HttpPost]
		//[Authorize("Authorization")]
		public async Task<IActionResult> EditRolePermission(string id, List<NavigationMenuViewModel> viewModel)
		{
			id = HttpContext.Session.GetObject<string>(Domain.Utility.SD.SessionRoleId);
			if (ModelState.IsValid)
			{
				var permissionIds = viewModel.Where(x => x.Permitted).Select(x => x.Id);
				await _dataAccessService.SetPermissionsByRoleIdAsync(id, permissionIds);
				return RedirectToAction(nameof(Roles));
			}
			return View(viewModel);
			//}
		}
		//[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account", new { area = "Identity" });
		}

		[HttpPost]
		public async Task<IActionResult> SaveTreeItems(string selectedItems)
		{
			List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

			var parents = items.Where(a => a.parent != "#");
						
			string id = HttpContext.Session.GetObject<string>(Domain.Utility.SD.SessionRoleId);
			
			//foreach(var chk in parents.ToList())
   //         {
			//	items.Add(chk);
   //         }
			
			var permissions = items.Select(a => a.id);
			
			var q = from all in items
					 join prnt in parents
					 on all.parent equals prnt.parent
					 select all.id;

			foreach (var item in items)
			{
				await _dataAccessService.SetPermissionsByRoleIdAsync(id, permissions);
				return RedirectToAction(nameof(Roles));
			}
			return RedirectToAction(nameof(Roles));
		}
	}
}
