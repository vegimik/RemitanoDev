using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RemitanoDevTask.Models;

namespace RemitanoDevTask.Components
{
    // ShoppingCartSummary
    public class UserSummary: ViewComponent
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserSummary(AppDbContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<IdentityUser> users = new List<IdentityUser>();
            IdentityUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
                users.Add(user);
            }
            
            
            return View(users);
        }
    }
}
