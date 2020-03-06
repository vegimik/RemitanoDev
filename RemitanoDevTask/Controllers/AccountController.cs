using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemitanoDevTask.Models;
using RemitanoDevTask.ViewModels;

namespace RemitanoDevTask.Controllers
{

    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        //if (User.IsInRole("Admin"))
                        //{
                        //    //return RedirectToAction("Index", "Admin");
                        //    return RedirectToAction("Index", "AppException");
                        //}
                        //else// if (User.IsInRole("UserMember"))
                        //{
                        //    return RedirectToAction("Index", "Home");
                        //}
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password is incorrect.");
                        return View(loginViewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This user doesn't exist.");
                    return View(loginViewModel);
                }
            }
            //ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (userExist != null)
                {
                    ModelState.AddModelError("", "This user exist");
                    return View(loginViewModel);
                }

                var user = new IdentityUser() { UserName = loginViewModel.UserName };
                var result = await _userManager.CreateAsync(user, loginViewModel.Password);

                if (result.Succeeded)
                {
                    await Initializer.initial(_roleManager);
                    if (loginViewModel.Lloji == 1)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "UserMember");
                        var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                        UserRegistered userRegistered = new UserRegistered()
                        {
                            UserRegisteredId = loginViewModel.UserName,
                            Email = loginViewModel.UserName
                        };

                        _context.Add(userRegistered);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
