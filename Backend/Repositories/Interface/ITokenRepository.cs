using System;
using Microsoft.AspNetCore.Identity;

namespace MatchDetailsApp.Repositories.Interface
{
	public interface ITokenRepository
	{
		string CreateJwtToken(IdentityUser User, List<string> roles);
	}
}

