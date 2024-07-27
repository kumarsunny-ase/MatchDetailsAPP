﻿// <auto-generated />
using System;
using MatchDetailsApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MatchDetailsApp.Migrations
{
    [DbContext(typeof(MatchDetailsDbContext))]
    [Migration("20240727060702_UpdateTimeProperty")]
    partial class UpdateTimeProperty
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MatchDetailsApp.Models.Domain.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("MatchDetailsApp.Models.Domain.Value", b =>
                {
                    b.Property<string>("MatchId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GuestTeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("MatchDay")
                        .HasColumnType("int");

                    b.Property<DateTime>("PlannedKickoffTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("StadiumName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MatchId");

                    b.HasIndex("ItemId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("MatchDetailsApp.Models.Domain.Value", b =>
                {
                    b.HasOne("MatchDetailsApp.Models.Domain.Item", "Item")
                        .WithMany("Values")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("MatchDetailsApp.Models.Domain.Item", b =>
                {
                    b.Navigation("Values");
                });
#pragma warning restore 612, 618
        }
    }
}
