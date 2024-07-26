using System;
using MatchDetailsApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatchDetailsApp.Data
{
	public class MatchDetailsDbContext : DbContext
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchDetailsDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public MatchDetailsDbContext(DbContextOptions<MatchDetailsDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Value> Values { get; set; }

        /// <summary>
        /// Override this method to configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary key for the Value entity
            modelBuilder.Entity<Value>()
                .HasKey(v => v.MatchId);

            // Configure one-to-many relationship between Item and Value
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Values)
                .WithOne(v => v.Item)
                .HasForeignKey(v => v.ItemId);

            base.OnModelCreating(modelBuilder);
        }

    }
}

