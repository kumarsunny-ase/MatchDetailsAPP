using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchDetailsApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewValueTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StadiumName",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamName",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GuestTeamName",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CompetitionId",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionName",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionType",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Values",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "MatchDateFixed",
                table: "Values",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MatchType",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StadiumId",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Values",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "CompetitionName",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "CompetitionType",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "MatchDateFixed",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "MatchType",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "StadiumId",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Values");

            migrationBuilder.AlterColumn<string>(
                name: "StadiumName",
                table: "Values",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamName",
                table: "Values",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GuestTeamName",
                table: "Values",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
