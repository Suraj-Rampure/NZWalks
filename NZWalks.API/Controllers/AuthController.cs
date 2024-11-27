using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }


        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityuser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName

            };


            var identityResult = await userManager.CreateAsync(identityuser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                //Add role to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRoleAsync(identityuser, registerRequestDto.Roles);
                }

                if (identityResult.Succeeded)
                {
                    return Ok("User was registered! Please Login");
                }

            }

            return BadRequest("Something went wrong");
            }

        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {

            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if(user!= null )
            {
              var CheckPasswordResult = await userManager.CheckPasswordAsync(user , loginRequestDto.Password);

                if(CheckPasswordResult)
                {
                    //create Token
                    return Ok();
                }
            }

            return BadRequest("UserName or Password Incorrect");
        }
    }
}
