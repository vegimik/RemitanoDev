using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemitanoDevTask.Models;

namespace RemitanoDevTask.Controllers
{

    public class UserRegisteredController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserRegisteredController(AppDbContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: 
        [Authorize(Roles = "Admin,UserMember")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
            var model = await _context.UsersRegistered.Where(p => p.UserRegisteredId.Contains(user.ToString())).ToListAsync();
            return View(model);
        }

        // GET: User/Profile     eshte vetem kjo per tek profili
        [Authorize(Roles = "Admin,UserMember")]
        public async Task<IActionResult> Profile()
        {

            var userRegistered = await _context.UsersRegistered
                .FirstOrDefaultAsync(m => m.UserRegisteredId == _userManager.GetUserId(HttpContext.User));
            if (userRegistered == null)
            {

                userRegistered.UserRegisteredId = _userManager.GetUserId(HttpContext.User);
                _context.Add(userRegistered);
                await _context.SaveChangesAsync();

                userRegistered = await _context.UsersRegistered
                .FirstOrDefaultAsync(m => m.UserRegisteredId == _userManager.GetUserId(HttpContext.User));

                //return NotFound();
            }

            return View(userRegistered);
        }

        // GET: User/Details/5
        [Authorize(Roles = "Admin,UserMember")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRegistered = await _context.UsersRegistered
                .FirstOrDefaultAsync(m => m.UserRegisteredId == id);
            if (userRegistered == null)
            {

                userRegistered.UserRegisteredId = _userManager.GetUserId(HttpContext.User);
                _context.Add(userRegistered);
                await _context.SaveChangesAsync();

                userRegistered = await _context.UsersRegistered.FindAsync(id);

                //return NotFound();
            }

            return View(userRegistered);
        }

        // GET: User/Create
        [Authorize(Roles = "Admin, UserMember")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, UserMember")]
        public async Task<IActionResult> Create([Bind("UserRegisteredId,Name,Surname,Email,PhoneNumber,DateOfBirth,Age")] UserRegistered userRegistered)
        {
            if (ModelState.IsValid)
            {
                userRegistered.UserRegisteredId = _userManager.GetUserId(HttpContext.User);
                _context.Add(userRegistered);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userRegistered);
        }

        // GET: User/Edit/5
        [Authorize(Roles = "Admin,UserMember")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRegistered = await _context.UsersRegistered.FindAsync(id);
            if (userRegistered == null)
            {
                return NotFound();
            }
            return View(userRegistered);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,UserMember")]
        public async Task<IActionResult> Edit([Bind("UserRegisteredId,Name,Surname,Email,PhoneNumber,DateOfBirth,Age")] UserRegistered userRegistered)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRegistered);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userRegistered.UserRegisteredId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userRegistered);
        }

        // GET: User/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRegistered = await _context.UsersRegistered
                .FirstOrDefaultAsync(m => m.UserRegisteredId == id);
            if (userRegistered == null)
            {
                return NotFound();
            }

            return View(userRegistered);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userRegistered = await _context.UsersRegistered.FindAsync(id);
            _context.UsersRegistered.Remove(userRegistered);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.UsersRegistered.Any(e => e.UserRegisteredId == id);
        }
    }
}
