using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Abstractions.Services;
using Pronia.Models;
using Pronia.Utilies.Enums;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {
		private readonly IEmailService _emailservice;

		UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager,IEmailService emailservice)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
			_emailservice = emailservice;
		}


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM regVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser()
            {
                Name = regVM.Name,
                Surname = regVM.Surname,
                UserName = regVM.Username,
                Email = regVM.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, regVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user,Roles.Names.Member.ToString());

            //Email conf
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, Email = user.Email }, Request.Scheme);
            _emailservice.Send(user.Email, "Confirmation link", confLink);
            
            return RedirectToAction(nameof(SuccessfullyRegistered));
        }

        public async Task<IActionResult> ConfirmEmail(string token,string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if(user is null || token is null || email is null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded) return BadRequest();
			await _signInManager.SignInAsync(user, true);
			return View();
        }
        public IActionResult SuccessfullyRegistered()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM,string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

            if(appUser is null)
            {
                appUser = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
                if(appUser is null)
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                    return View();
                }
            }
            if (!appUser.EmailConfirmed)
            {
				ModelState.AddModelError("", "Please Confirmed your Email");
				return View();
			}
            var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.IsPresistance,true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }

            if(ReturnUrl is not null)
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index","Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //public async Task<IActionResult> Test()
        //{
        //    var user = await _userManager.FindByNameAsync("sabir");
        //    await _userManager.AddToRoleAsync(user,Roles.Names.Superadmin.ToString());
        //    user = await _userManager.FindByNameAsync("tarlan");
        //    await _userManager.AddToRoleAsync(user, Roles.Names.Admin.ToString());
        //    return View();
        //}

        public async Task<IActionResult> AddRole()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Superadmin" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            return View();
        }
    }
}
