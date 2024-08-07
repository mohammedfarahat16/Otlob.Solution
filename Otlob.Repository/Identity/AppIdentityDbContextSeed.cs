

using Microsoft.AspNetCore.Identity;
using Otlob.Core.Entites.Identity;

namespace Otlob.Repository.Identity
{

    public static class AppIdentityDbContextSeed 
    {


        public static async Task SeedUserAsync(UserManager<AppUser> userManger) 
        {
            if (!userManger.Users.Any()) 
            {
                var User = new AppUser() 
                {
                    DisplayName= "Mohammed Farahat",
                    Email = "mohammedfarahat@gmail.com",
                    UserName = "mohammedfarahat",
                    PhoneNumber = "01222332312"
                };

                await userManger.CreateAsync(User,"Pa$$w0rd");




            }
        }
    }
}
