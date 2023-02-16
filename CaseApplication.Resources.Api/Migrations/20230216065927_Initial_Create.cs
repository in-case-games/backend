using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    GroupCasesName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GameCaseCost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    GameCaseBalance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
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
                    GameItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GameItemCost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    GameItemImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameItemRarity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NewsContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromocodeType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromocodeTypeName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocodeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersCount = table.Column<int>(type: "int", nullable: false),
                    SiteBalance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    CasesOpened = table.Column<int>(type: "int", nullable: false),
                    ItemWithdrawn = table.Column<int>(type: "int", nullable: false),
                    ReviewsWriten = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserLogin = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    UserImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    LossChance = table.Column<int>(type: "int", nullable: false)
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
                name: "Promocode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromocodeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromocodeName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PromocodeDiscount = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    PromocodeUsesCount = table.Column<int>(type: "int", nullable: false),
                    PromocodeExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promocode_PromocodeType_PromocodeTypeId",
                        column: x => x.PromocodeTypeId,
                        principalTable: "PromocodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryOpeningCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseOpenAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistoryOpeningCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHistoryOpeningCases_GameCase_GameCaseId",
                        column: x => x.GameCaseId,
                        principalTable: "GameCase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHistoryOpeningCases_GameItem_GameItemId",
                        column: x => x.GameItemId,
                        principalTable: "GameItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHistoryOpeningCases_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPlatfrom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
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
                    UserBalance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    UserAbleToPay = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    IsConfirmedAccount = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PromocodeUsedByUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromocodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocodeUsedByUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromocodeUsedByUsers_Promocode_PromocodeId",
                        column: x => x.PromocodeId,
                        principalTable: "Promocode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromocodeUsedByUsers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "Id", "PromocodeTypeName" },
                values: new object[,]
                {
                    { new Guid("401c8471-306b-4ee9-9598-65b4aed479e1"), "balance" },
                    { new Guid("d2a9c0d7-5508-460c-b244-cf95c2311c50"), "case" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { new Guid("22d463a0-5c31-4aea-a0dd-bce0c7815082"), "user" },
                    { new Guid("e22655e2-7111-4089-bfe3-f1c546f96091"), "admin" }
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
                name: "IX_News_Id",
                table: "News",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promocode_Id",
                table: "Promocode",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promocode_PromocodeName",
                table: "Promocode",
                column: "PromocodeName",
                unique: true,
                filter: "[PromocodeName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Promocode_PromocodeTypeId",
                table: "Promocode",
                column: "PromocodeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PromocodeType_Id",
                table: "PromocodeType",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromocodeType_PromocodeTypeName",
                table: "PromocodeType",
                column: "PromocodeTypeName",
                unique: true,
                filter: "[PromocodeTypeName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PromocodeUsedByUsers_Id",
                table: "PromocodeUsedByUsers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromocodeUsedByUsers_PromocodeId",
                table: "PromocodeUsedByUsers",
                column: "PromocodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PromocodeUsedByUsers_UserId",
                table: "PromocodeUsedByUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteStatistics_Id",
                table: "SiteStatistics",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PasswordSalt",
                table: "User",
                column: "PasswordSalt",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserEmail",
                table: "User",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserLogin",
                table: "User",
                column: "UserLogin",
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
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistoryOpeningCases_GameCaseId",
                table: "UserHistoryOpeningCases",
                column: "GameCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistoryOpeningCases_GameItemId",
                table: "UserHistoryOpeningCases",
                column: "GameItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistoryOpeningCases_Id",
                table: "UserHistoryOpeningCases",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserHistoryOpeningCases_UserId",
                table: "UserHistoryOpeningCases",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseInventory");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "PromocodeUsedByUsers");

            migrationBuilder.DropTable(
                name: "SiteStatistics");

            migrationBuilder.DropTable(
                name: "UserAdditionalInfo");

            migrationBuilder.DropTable(
                name: "UserHistoryOpeningCases");

            migrationBuilder.DropTable(
                name: "UserInventory");

            migrationBuilder.DropTable(
                name: "UserRestriction");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "Promocode");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "GameCase");

            migrationBuilder.DropTable(
                name: "GameItem");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PromocodeType");
        }
    }
}
