using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchDetailsApp.Data
{
	public class AuthDbContext : IdentityDbContext
	{
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //Seed Role
            var userRoleId = "c4db538a-6e12-4520-9de9-09fd3e682cd5";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Seed User
            var userId = "5939642e-9490-4f7d-8827-238f0e4df058";
            var user = new IdentityUser
            {
                UserName = "user@gmail.com",
                Email = "user@gmail.com",
                NormalizedEmail = "user@gmail.com".ToUpper(),
                NormalizedUserName = "user@gmail.com".ToUpper(),
                Id = userId
            };

            user.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(user, "User@123");

            builder.Entity<IdentityUser>().HasData(user);

            // Add All roles to User
            var userRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = userId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }

        
    }
}

