using Casgem_IdentityRole.DAL;
using Casgem_IdentityRole.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Casgem_IdentityRole.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username,model.Password,false, false);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "UserList");
            }
            return View();
        }
    }
}
