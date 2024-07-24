using System;
using MatchDetailsApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatchDetailsApp.Data
{
	public class MatchDetailsDbContext : DbContext
	{
        public MatchDetailsDbContext(DbContextOptions<MatchDetailsDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Value> Values { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

