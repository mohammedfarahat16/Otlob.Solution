using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Otlob.Core.Entites.Identity;
using Otlob.Core.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager)
        {
            //Payload Data , claims

            //1. private claim

            var AuthClaims = new List<Claim>() 
            {
                new Claim(ClaimTypes.GivenName , user.DisplayName),
                new Claim (ClaimTypes.Email,user.Email)
            };


            var UserRoles = await userManager.GetRolesAsync(user);
            foreach (var role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            //2.regiser claim


            var AuthKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(configuration["JWT:Key"]) );




            var Token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience : configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:Duration"])),
                claims : AuthClaims,
                signingCredentials : new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }



    }
}
