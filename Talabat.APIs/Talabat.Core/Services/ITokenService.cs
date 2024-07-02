using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Models.Identity;

namespace Talabat.Core.Services
{
    public interface ITokenService
    {
        //signature for function
        Task<string> CreateTokenAsync (AppUser user, UserManager<AppUser> userManager);
    }
}
