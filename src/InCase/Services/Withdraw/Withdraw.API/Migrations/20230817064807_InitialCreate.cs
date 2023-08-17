using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Withdraw.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_withdraw_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    id_for_market = table.Column<string>(type: "text", nullable: false),
                    cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_game_item_game_game_id",
                        column: x => x.game_id,
                        principalTable: "Game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameMarket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_market", x => x.id);
                    table.ForeignKey(
                        name: "fk_game_market_game_game_id",
                        column: x => x.game_id,
                        principalTable: "Game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fixed_cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_inventory", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_inventory_game_item_item_id",
                        column: x => x.item_id,
                        principalTable: "GameItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_inventory_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryWithdraw",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<string>(type: "text", nullable: false),
                    fixed_cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    market_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_history_withdraw", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_history_withdraw_game_item_item_id",
                        column: x => x.item_id,
                        principalTable: "GameItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_history_withdraw_game_market_market_id",
                        column: x => x.market_id,
                        principalTable: "GameMarket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_history_withdraw_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "WithdrawStatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_history_withdraw_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1d59abf1-ac51-46ef-9fe8-a1d3b1f80ad9"), "csgo" },
                    { new Guid("8965bfb0-9af1-4362-adb5-7359f4e89a9d"), "dota2" }
                });

            migrationBuilder.InsertData(
                table: "WithdrawStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2f55a378-f701-4808-a13b-72592ba3b0e7"), "transfer" },
                    { new Guid("3b9ed0cb-9971-4d3a-9911-894ceacdd49f"), "given" },
                    { new Guid("d835f224-ddf1-487b-b4d6-d3e4ca0c3e85"), "purchase" },
                    { new Guid("de1a7a6a-6d20-4fd9-94d3-361c9e2ec56c"), "cancel" }
                });

            migrationBuilder.InsertData(
                table: "GameMarket",
                columns: new[] { "id", "game_id", "name" },
                values: new object[,]
                {
                    { new Guid("286b451c-1dfc-4be0-9f25-05c3974c3445"), new Guid("1d59abf1-ac51-46ef-9fe8-a1d3b1f80ad9"), "tm" },
                    { new Guid("64f32a58-ceaa-44ce-b3ac-d4018917b4a3"), new Guid("8965bfb0-9af1-4362-adb5-7359f4e89a9d"), "tm" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_id",
                table: "Game",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_name",
                table: "Game",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_game_id",
                table: "GameItem",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_item_id",
                table: "GameItem",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_market_game_id",
                table: "GameMarket",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_market_id",
                table: "GameMarket",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_market_name",
                table: "GameMarket",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_id",
                table: "UserHistoryWithdraw",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_invoice_id",
                table: "UserHistoryWithdraw",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_item_id",
                table: "UserHistoryWithdraw",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_market_id",
                table: "UserHistoryWithdraw",
                column: "market_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_status_id",
                table: "UserHistoryWithdraw",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_user_id",
                table: "UserHistoryWithdraw",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_inventory_id",
                table: "UserInventory",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_inventory_item_id",
                table: "UserInventory",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_inventory_user_id",
                table: "UserInventory",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_withdraw_status_id",
                table: "WithdrawStatus",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_withdraw_status_name",
                table: "WithdrawStatus",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHistoryWithdraw");

            migrationBuilder.DropTable(
                name: "UserInventory");

            migrationBuilder.DropTable(
                name: "GameMarket");

            migrationBuilder.DropTable(
                name: "WithdrawStatus");

            migrationBuilder.DropTable(
                name: "GameItem");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
