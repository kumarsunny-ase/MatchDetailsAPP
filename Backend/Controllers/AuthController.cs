using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchDetailsApp.Models.DTOs;
using MatchDetailsApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MatchDetailsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controller for handling user authentication tasks such as login and registration.
    /// </summary>
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenRepository _tokenRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager for handling user-related operations.</param>
        /// <param name="tokenRepository">The token repository for handling JWT tokens.</param>
        public AuthController(UserManager<IdentityUser> userManager, TokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Logs in a user by validating their email and password and generating a JWT token.
        /// </summary>
        /// <param name="request">The login request containing the user's email and password.</param>
        /// <returns>A response with the user's email, roles, and JWT token if successful; otherwise, a validation problem response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var identityUser = await _userManager.FindByEmailAsync(request.Email);

                if (identityUser is not null)
                {
                    var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, request.Password);

                    if (checkPasswordResult)
                    {
                        var roles = await _userManager.GetRolesAsync(identityUser);

                        //Create a Token and Response
                        var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                        var response = new LoginResponseDto()
                        {
                            Email = request.Email,
                            Roles = roles.ToList(),
                            Token = jwtToken
                        };

                        return Ok(response);
                    }
                }

                ModelState.AddModelError("", "Email or Password Incorrect");
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred. Please try again later : {ex.Message}");
            }
            
        }

        /// <summary>
        /// Registers a new user with the provided email and password.
        /// </summary>
        /// <param name="request">The registration request containing the user's email and password.</param>
        /// <returns>A response indicating the result of the registration operation.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = request.Email?.Trim(),
                    Email = request.Email?.Trim()
                };

                var identityResult = await _userManager.CreateAsync(user, request.Password);

                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddToRoleAsync(user, "User");

                    if (identityResult.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        if (identityResult.Errors.Any())
                        {
                            // Adding model state errors from AddToRoleAsync
                            foreach (var error in identityResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        // Adding model state errors from CreateAsync
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                // Return a 500 status code with error message if an unexpected error occurs
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred. Please try again later: {ex.Message}");
            }
            
        }
    }
}

