using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LootBox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    virtual_balance = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false),
                    is_active_banner = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loot_box", x => x.id);
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
                name: "LootBoxInventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chance_wining = table.Column<int>(type: "integer", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    box_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loot_box_inventory", x => x.id);
                    table.ForeignKey(
                        name: "fk_loot_box_inventory_game_item_item_id",
                        column: x => x.item_id,
                        principalTable: "GameItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_loot_box_inventory_loot_box_box_id",
                        column: x => x.box_id,
                        principalTable: "LootBox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionalInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    is_guest_mode = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_additional_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_additional_info_users_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOpening",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    box_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_opening", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_opening_game_item_item_id",
                        column: x => x.item_id,
                        principalTable: "GameItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_opening_loot_box_box_id",
                        column: x => x.box_id,
                        principalTable: "LootBox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_opening_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPathBanner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number_steps = table.Column<int>(type: "integer", nullable: false),
                    fixed_cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    box_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_path_banner", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_path_banner_game_item_item_id",
                        column: x => x.item_id,
                        principalTable: "GameItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_path_banner_loot_box_box_id",
                        column: x => x.box_id,
                        principalTable: "LootBox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_path_banner_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPromocode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_promocode", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_promocode_users_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_item_id",
                table: "GameItem",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_id",
                table: "LootBox",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_inventory_box_id",
                table: "LootBoxInventory",
                column: "box_id");

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_inventory_id",
                table: "LootBoxInventory",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_inventory_item_id",
                table: "LootBoxInventory",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_additional_info_id",
                table: "UserAdditionalInfo",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_additional_info_user_id",
                table: "UserAdditionalInfo",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_opening_box_id",
                table: "UserOpening",
                column: "box_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_opening_id",
                table: "UserOpening",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_opening_item_id",
                table: "UserOpening",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_opening_user_id",
                table: "UserOpening",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_path_banner_box_id",
                table: "UserPathBanner",
                column: "box_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_path_banner_id",
                table: "UserPathBanner",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_path_banner_item_id",
                table: "UserPathBanner",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_path_banner_user_id",
                table: "UserPathBanner",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_promocode_id",
                table: "UserPromocode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_promocode_user_id",
                table: "UserPromocode",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LootBoxInventory");

            migrationBuilder.DropTable(
                name: "UserAdditionalInfo");

            migrationBuilder.DropTable(
                name: "UserOpening");

            migrationBuilder.DropTable(
                name: "UserPathBanner");

            migrationBuilder.DropTable(
                name: "UserPromocode");

            migrationBuilder.DropTable(
                name: "GameItem");

            migrationBuilder.DropTable(
                name: "LootBox");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
