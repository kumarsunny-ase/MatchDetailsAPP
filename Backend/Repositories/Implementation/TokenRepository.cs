using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MatchDetailsApp.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MatchDetailsApp.Repositories
{
    /// <summary>
    /// Repository for generating JWT tokens.
    /// </summary>
    public class TokenRepository : ITokenRepository
	{
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration used to retrieve JWT settings.</param>
        public TokenRepository(IConfiguration configuration)
		{
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a JWT token for the specified user and roles.
        /// </summary>
        /// <param name="user">The user for whom the token is being created.</param>
        /// <param name="roles">The list of roles associated with the user.</param>
        /// <returns>A JWT token as a string.</returns>
        public string CreateJwtToken(IdentityUser user, List<string> roles)
		{
			// Create Claims
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, user.Email)
			};

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			//JWT Security Token Parameters
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(15), // Token expiration time
				signingCredentials: credentials);

            // // Serialize the token to a string
            return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

