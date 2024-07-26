using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchDetailsApp.Data
{
	public class AuthDbContext : IdentityDbContext
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthDbContext"/> class.
        /// </summary>
        /// <param name="options">The options used to configure the database context.</param>
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // <summary>
        /// Configures the model for the database context, including seeding initial data for roles and users.
        /// </summary>
        /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //Seed initial Role
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

            // Seed initial User
            var userId = "5939642e-9490-4f7d-8827-238f0e4df058";
            var user = new IdentityUser
            {
                UserName = "user@gmail.com",
                Email = "user@gmail.com",
                NormalizedEmail = "user@gmail.com".ToUpper(),
                NormalizedUserName = "user@gmail.com".ToUpper(),
                Id = userId
            };

            // Hash the password for the initial user
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

