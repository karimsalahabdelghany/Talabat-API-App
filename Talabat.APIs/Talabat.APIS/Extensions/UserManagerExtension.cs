using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Models.Identity;

namespace Talabat.APIS.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?>FindUserWithAddressAsync(this UserManager<AppUser>userManager,ClaimsPrincipal User)
        {
           var email = User.FindFirstValue(ClaimTypes.Email);
           var user =await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(u => u.Email == email);
            return user;
            


        }
    }
}
