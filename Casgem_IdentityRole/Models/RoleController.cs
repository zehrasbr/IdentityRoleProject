using Casgem_IdentityRole.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Casgem_IdentityRole.Models
{
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult UserList()
        {
            var values = _userManager.Users.ToList();
            return View(values);
        }
        public IActionResult RoleList()
        {
            var values = _roleManager.Roles.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleAddViewModel model)
        {
            AppRole appRole = new AppRole()
            {
                Name = model.Name,
            };
            var result = await _roleManager.CreateAsync(appRole);
            if (result.Succeeded)
            {
                return RedirectToAction("RoleList");
            }
            return View();
        }
        public async Task<IActionResult> DeleteRole(int id)
        {
            var value = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            await _roleManager.DeleteAsync(value);
            return RedirectToAction("RoleList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRole(int id)
        {
            var value = await _roleManager.Roles.FirstOrDefaultAsync( x => x.Id == id);
            UpdateRoleViewModel model = new UpdateRoleViewModel()
            {
                Id = value.Id,
                Name = value.Name,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            var value = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == model.Id);
            value.Name= model.Name;
            await _roleManager.UpdateAsync(value);
            return RedirectToAction("RoleList");
        }
        [HttpGet]
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.Id == id);
            TempData["userid"] = user.Id;
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();
            foreach (var item in roles)
            {
                RoleAssignViewModel model = new RoleAssignViewModel();
                model.RoleID = item.Id;
                model.RoleName = item.Name;
                model.RoleExist = userRoles.Contains(item.Name);
                roleAssignViewModels.Add(model);
            }
            return View(roleAssignViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> model)
        {
            var userid = (int)TempData["userid"];
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userid);
            foreach (var item in model) 
            {
                if(item.RoleExist)
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }
            return RedirectToAction("UserList");
        }
    }
}
