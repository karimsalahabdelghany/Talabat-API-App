using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
   
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager , 
            SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>>Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)    // transform function to Syncrouns to block funtion after it
            {
                return BadRequest(new ApiResponse(400, "Email is ALready in Use"));
            }

            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber

            };
           
            var Resualt = await _userManager.CreateAsync(User, model.Password);
            if(!Resualt.Succeeded) return BadRequest(new ApiResponse(400));

            var ReturnedsUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token =await _tokenService.CreateTokenAsync(User,_userManager)
            };
            return Ok(ReturnedsUser);
        }

        private object? ApiResponse(int v1, string v2)
        {
            throw new NotImplementedException();
        }

        //Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto model)
        {
            var User =await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
           var Resualt = await _signInManager.CheckPasswordSignInAsync(User, model.Password,false);
            if (!Resualt.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token =await _tokenService.CreateTokenAsync(User, _userManager)
            }) ;
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>>GetCurrentUser()
        
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user =await _userManager.FindByEmailAsync(Email);
            var RetunedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(RetunedUser);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>>GetCurrentAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            //var user =await  _userManager.FindByEmailAsync(Email);
            var user  =await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user?.Address);
            return Ok(MappedAddress);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>>UpdateAddress(AddressDto UpdatedAddress)
        {
           var email = User.FindFirstValue(ClaimTypes.Email);
           var user =await _userManager.FindUserWithAddressAsync(User);
           var MappedAddress = _mapper.Map<AddressDto,Address>(UpdatedAddress);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
           var Resualt =await  _userManager.UpdateAsync(user);
            if (!Resualt.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);
           

        }
        [HttpGet("EmailExists")]
        //baseUrl/Api/Accounts/EmailExists
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            //var user =await _userManager.FindByEmailAsync(email);
            //if (user is not null) return true;
            //else return false;
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
