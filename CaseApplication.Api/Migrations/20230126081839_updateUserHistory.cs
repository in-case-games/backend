using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateUserHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CaseOpenAt",
                table: "UserHistoryOpeningCases",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseOpenAt",
                table: "UserHistoryOpeningCases");
        }
    }
}
