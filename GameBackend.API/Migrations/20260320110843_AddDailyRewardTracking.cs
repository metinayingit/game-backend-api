using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBackend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyRewardTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastDailyRewardClaim",
                table: "Players",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDailyRewardClaim",
                table: "Players");
        }
    }
}
