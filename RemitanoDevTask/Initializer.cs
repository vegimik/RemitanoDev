using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RemitanoDevTask
{
    public static class Initializer
    {
        public static async Task initial(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var users = new IdentityRole("Admin");
                await roleManager.CreateAsync(users);
            }
            if (!await roleManager.RoleExistsAsync("UserMember"))
            {
                var users = new IdentityRole("UserMember");
                await roleManager.CreateAsync(users);
            }
        }
    }
}