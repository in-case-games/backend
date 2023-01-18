using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.API.Migrations
{
    /// <inheritdoc />
    public partial class RefactorEasy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction");

            migrationBuilder.DropIndex(
                name: "IX_UserInventory_GameItemId",
                table: "UserInventory");

            migrationBuilder.DropIndex(
                name: "IX_UserInventory_UserId",
                table: "UserInventory");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "UserAdditionalInfo",
                newName: "UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAdditionalInfo_RoleId",
                table: "UserAdditionalInfo",
                newName: "IX_UserAdditionalInfo_UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalInfo_UserRole_UserRoleId",
                table: "UserAdditionalInfo",
                column: "UserRoleId",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalInfo_User_UserId",
                table: "UserAdditionalInfo",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalInfo_UserRole_UserRoleId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalInfo_User_UserId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction");

            migrationBuilder.RenameColumn(
                name: "UserRoleId",
                table: "UserAdditionalInfo",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAdditionalInfo_UserRoleId",
                table: "UserAdditionalInfo",
                newName: "IX_UserAdditionalInfo_RoleId");

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
                name: "IX_UserInventory_UserId",
                table: "UserInventory",
                column: "UserId",
                unique: true);
        }
    }
}
