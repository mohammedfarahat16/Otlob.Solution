using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otlob.APIs.DTOs;
using Otlob.APIs.Errors;
using Otlob.APIs.Extensions;
using Otlob.Core.Entites.Identity;
using Otlob.Core.Service;
using System.Security.Claims;

namespace Otlob.APIs.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountsController(UserManager<AppUser> _userManager,
            SignInManager<AppUser > _signInManager,
            ITokenService _service,
            IMapper _mapper
            )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            tokenService = _service;
            mapper = _mapper;
        }

        //register

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model) 
        {
            if (CheckEmaiExist(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400,"This Email is Already in use "));

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };

            var Result =  await userManager.CreateAsync(user,model.Password);

            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));



            var UserToReturn = new UserDto() 
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)

            };
            return Ok(UserToReturn);

        }

        //login

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model) 
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(user,model.Password,false);


            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto() {
                DisplayName= user.DisplayName,
                Email= user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            });
        }



        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            };

            return Ok(ReturnedUser);

        
        }








        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await userManager.FindUserWithAddressAsync(User);

            var MappedAddress = mapper.Map< Address, AddressDto>(user.Address);


            return Ok(MappedAddress);


        }



        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress) 
        {
            var user = await userManager.FindUserWithAddressAsync(User);

            if (user is null) return Unauthorized(new ApiResponse(401));

            var address = mapper.Map<AddressDto, Address>(UpdatedAddress);
            address.Id=user.Address.Id;
            user.Address = address;

            var Result = await userManager.UpdateAsync(user);

            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(UpdatedAddress);



        }



        [Authorize]
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmaiExist(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null) return false;
            else
                return true;

        }


    }
}
