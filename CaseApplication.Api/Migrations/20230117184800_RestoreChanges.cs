using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.API.Migrations
{
    /// <inheritdoc />
    public partial class RestoreChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseInventory_GameCase_GameCaseId",
                table: "CaseInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseInventory_GameItem_CaseItemId",
                table: "CaseInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalInfo_UserRole_UserRoleId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalInfo_User_UserId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInventory_GameItem_GameItemId",
                table: "UserInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInventory_User_UserId",
                table: "UserInventory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "UserRestriction",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 1, 17, 9, 50, 12, 997, DateTimeKind.Local).AddTicks(6));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserInventory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameItemId",
                table: "UserInventory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserRoleId",
                table: "UserAdditionalInfo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserAdditionalInfo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameCaseId",
                table: "CaseInventory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CaseItemId",
                table: "CaseInventory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInventory_GameCase_GameCaseId",
                table: "CaseInventory",
                column: "GameCaseId",
                principalTable: "GameCase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInventory_GameItem_CaseItemId",
                table: "CaseInventory",
                column: "CaseItemId",
                principalTable: "GameItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalInfo_UserRole_UserRoleId",
                table: "UserAdditionalInfo",
                column: "UserRoleId",
                principalTable: "UserRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalInfo_User_UserId",
                table: "UserAdditionalInfo",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInventory_GameItem_GameItemId",
                table: "UserInventory",
                column: "GameItemId",
                principalTable: "GameItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInventory_User_UserId",
                table: "UserInventory",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseInventory_GameCase_GameCaseId",
                table: "CaseInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseInventory_GameItem_CaseItemId",
                table: "CaseInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalInfo_UserRole_UserRoleId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalInfo_User_UserId",
                table: "UserAdditionalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInventory_GameItem_GameItemId",
                table: "UserInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInventory_User_UserId",
                table: "UserInventory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "UserRestriction",
                type: "datetime2(7)",
                nullable: true,
                defaultValue: new DateTime(2023, 1, 17, 9, 50, 12, 997, DateTimeKind.Local).AddTicks(6),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserInventory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GameItemId",
                table: "UserInventory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserRoleId",
                table: "UserAdditionalInfo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserAdditionalInfo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GameCaseId",
                table: "CaseInventory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CaseItemId",
                table: "CaseInventory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInventory_GameCase_GameCaseId",
                table: "CaseInventory",
                column: "GameCaseId",
                principalTable: "GameCase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInventory_GameItem_CaseItemId",
                table: "CaseInventory",
                column: "CaseItemId",
                principalTable: "GameItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserInventory_GameItem_GameItemId",
                table: "UserInventory",
                column: "GameItemId",
                principalTable: "GameItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInventory_User_UserId",
                table: "UserInventory",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
