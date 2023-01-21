using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class SomeChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CaseInventory_GameCaseId",
                table: "CaseInventory");

            migrationBuilder.DropIndex(
                name: "IX_CaseInventory_GameItemId",
                table: "CaseInventory");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_GameCaseId",
                table: "CaseInventory",
                column: "GameCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_GameItemId",
                table: "CaseInventory",
                column: "GameItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CaseInventory_GameCaseId",
                table: "CaseInventory");

            migrationBuilder.DropIndex(
                name: "IX_CaseInventory_GameItemId",
                table: "CaseInventory");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_GameCaseId",
                table: "CaseInventory",
                column: "GameCaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_GameItemId",
                table: "CaseInventory",
                column: "GameItemId",
                unique: true);
        }
    }
}
