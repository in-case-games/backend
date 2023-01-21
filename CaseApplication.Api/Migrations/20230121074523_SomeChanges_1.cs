using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class SomeChanges1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GameCase_GameCaseName",
                table: "GameCase",
                column: "GameCaseName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameCase_GameCaseName",
                table: "GameCase");
        }
    }
}
