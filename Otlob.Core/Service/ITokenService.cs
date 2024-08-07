using Microsoft.AspNetCore.Identity;
using Otlob.Core.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Service
{
    public interface ITokenService
    {

        Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);
    }
}
