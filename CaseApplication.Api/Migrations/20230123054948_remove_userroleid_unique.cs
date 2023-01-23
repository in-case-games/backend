using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class removeuserroleidunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameCase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameCaseName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GameCaseCost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    GameCaseImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevenuePrecentage = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameItemName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GameItemCost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    GameItemImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameItemRarity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    UserImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberItemsCase = table.Column<int>(type: "int", nullable: false),
                    LossChance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseInventory_GameCase_GameCaseId",
                        column: x => x.GameCaseId,
                        principalTable: "GameCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseInventory_GameItem_GameItemId",
                        column: x => x.GameItemId,
                        principalTable: "GameItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInventory_GameItem_GameItemId",
                        column: x => x.GameItemId,
                        principalTable: "GameItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInventory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRestriction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestrictionName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRestriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRestriction_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionalInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserAge = table.Column<int>(type: "int", nullable: false),
                    UserBalance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    UserAbleToPay = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdditionalInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAdditionalInfo_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAdditionalInfo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_GameCaseId",
                table: "CaseInventory",
                column: "GameCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_GameItemId",
                table: "CaseInventory",
                column: "GameItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInventory_Id",
                table: "CaseInventory",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameCase_GameCaseName",
                table: "GameCase",
                column: "GameCaseName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameCase_Id",
                table: "GameCase",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameItem_GameItemName",
                table: "GameItem",
                column: "GameItemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameItem_Id",
                table: "GameItem",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserEmail",
                table: "User",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalInfo_Id",
                table: "UserAdditionalInfo",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalInfo_UserId",
                table: "UserAdditionalInfo",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalInfo_UserRoleId",
                table: "UserAdditionalInfo",
                column: "UserRoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInventory_GameItemId",
                table: "UserInventory",
                column: "GameItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInventory_Id",
                table: "UserInventory",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInventory_UserId",
                table: "UserInventory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRestriction_Id",
                table: "UserRestriction",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRestriction_UserId",
                table: "UserRestriction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Id",
                table: "UserRole",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseInventory");

            migrationBuilder.DropTable(
                name: "UserAdditionalInfo");

            migrationBuilder.DropTable(
                name: "UserInventory");

            migrationBuilder.DropTable(
                name: "UserRestriction");

            migrationBuilder.DropTable(
                name: "GameCase");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "GameItem");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
