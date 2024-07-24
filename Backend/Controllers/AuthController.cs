using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchDetailsApp.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MatchDetailsApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email);

            if(identityUser is not null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, request.Password);

                if(checkPasswordResult)
                {
                    var roles = await _userManager.GetRolesAsync(identityUser);

                    //Create a Token and Response

                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = "TOKEN"
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password Incorrect");

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if(identityResult.Succeeded)
            {
                identityResult = await _userManager.AddToRoleAsync(user, "User");

                if(identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if(identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}

