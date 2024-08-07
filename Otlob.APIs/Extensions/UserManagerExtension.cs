using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Otlob.Core.Entites.Identity;
using System.Security.Claims;

namespace Otlob.APIs.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u=>u.Email==email);

            return user;
        }
    }
}
