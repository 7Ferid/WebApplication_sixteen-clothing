using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication_sixteen_clothing.Models;
using WebApplication_sixteen_clothing.ViewModels.UserViewModels;

namespace WebApplication_sixteen_clothing.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager,RoleManager<IdentityRole> _roleManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            AppUser user = new()
            {
                Email = vm.Email,
                UserName = vm.UserName,
                FullName = vm.FullName
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            //rolebacki yazandan sonra bunu yaziriq 
            await _userManager.AddToRoleAsync(user, "Member");




            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var User = await _userManager.FindByEmailAsync(vm.Email);
            if(User is null)
            {
                ModelState.AddModelError("", "Username or password is wrong ");
                return View(vm);
            }
          var result=  await _signInManager.PasswordSignInAsync(User, vm.Password, false, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name="Admin"
            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Member"
            });
            return Ok("Role was Created")
        }

    }
}
