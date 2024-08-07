using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Otlob.Core.Entites.Identity;
using Otlob.Core.Service;
using Otlob.Repository.Identity;
using Otlob.Service;
using System.Text;

namespace Otlob.APIs.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration) 
        {
            Services.AddIdentity<AppUser, IdentityRole>(Options =>
            {
                //Options.Password.RequireLowercase= true;
            })
                           .AddEntityFrameworkStores<AppIdentityDbContext>();

            Services.AddAuthentication(Opetions => 
            {
                Opetions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Opetions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(Options => 
                    {
                        Options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JWT:ValidIssuer"],
                            ValidateAudience = true,
                            ValidAudience = configuration["JWT:ValidAudience"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                        };
                    });

            Services.AddScoped<ITokenService, TokenService>();

            return Services;
        }
    }
}
