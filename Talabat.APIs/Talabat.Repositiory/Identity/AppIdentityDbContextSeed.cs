using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Models.Identity;

namespace Talabat.Repositiory.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "KarimSalah",
                    Email = "Karimabdelghany753@gmail.com",
                    UserName = "KarimSalah",
                    PhoneNumber = "01276823186",
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
            
        }
    }
}
