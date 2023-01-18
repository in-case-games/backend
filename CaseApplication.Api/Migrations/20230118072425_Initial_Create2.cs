using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Id",
                table: "UserRole",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRestriction_Id",
                table: "UserRestriction",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInventory_GameItemId",
                table: "UserInventory",
                column: "GameItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInventory_Id",
                table: "UserInventory",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInventory_UserId",
                table: "UserInventory",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalInfo_Id",
                table: "UserAdditionalInfo",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalInfo_RoleId",
                table: "UserAdditionalInfo",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalInfo_UserId",
                table: "UserAdditionalInfo",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameItem_Id",
                table: "GameItem",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameCase_Id",
                table: "GameCase",
                column: "Id",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_Id",
                table: "CaseInventory",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRole_Id",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRestriction_Id",
                table: "UserRestriction");

            migrationBuilder.DropIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction");

            migrationBuilder.DropIndex(
                name: "IX_UserInventory_GameItemId",
                table: "UserInventory");

            migrationBuilder.DropIndex(
                name: "IX_UserInventory_Id",
                table: "UserInventory");

            migrationBuilder.DropIndex(
                name: "IX_UserInventory_UserId",
                table: "UserInventory");

            migrationBuilder.DropIndex(
                name: "IX_UserAdditionalInfo_Id",
                table: "UserAdditionalInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserAdditionalInfo_RoleId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserAdditionalInfo_UserId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropIndex(
                name: "IX_User_Id",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_GameItem_Id",
                table: "GameItem");

            migrationBuilder.DropIndex(
                name: "IX_GameCase_Id",
                table: "GameCase");

            migrationBuilder.DropIndex(
                name: "IX_CaseInventory_GameCaseId",
                table: "CaseInventory");

            migrationBuilder.DropIndex(
                name: "IX_CaseInventory_GameItemId",
                table: "CaseInventory");

            migrationBuilder.DropIndex(
                name: "IX_CaseInventory_Id",
                table: "CaseInventory");

            migrationBuilder.CreateIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction",
                column: "UserId");
        }
    }
}
