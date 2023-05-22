using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Resources.API.Migrations
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
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemQuality",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item_quality", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemRarity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item_rarity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupLootBox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_loot_box", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LootBox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loot_box", x => x.id);
                    table.ForeignKey(
                        name: "fk_loot_box_game_game_id",
                        column: x => x.game_id,
                        principalTable: "Game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    hash_name = table.Column<string>(type: "text", nullable: true),
                    cost = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    rarity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quality_id = table.Column<Guid>(type: "uuid", nullable: true)
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
                    table.ForeignKey(
                        name: "fk_game_item_item_qualities_quality_id",
                        column: x => x.quality_id,
                        principalTable: "GameItemQuality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_item_item_rarities_rarity_id",
                        column: x => x.rarity_id,
                        principalTable: "GameItemRarity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_item_item_types_type_id",
                        column: x => x.type_id,
                        principalTable: "GameItemType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LootBoxBanner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    box_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loot_box_banner", x => x.id);
                    table.ForeignKey(
                        name: "fk_loot_box_banner_loot_boxes_box_id",
                        column: x => x.box_id,
                        principalTable: "LootBox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LootBoxGroup",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    box_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loot_box_group", x => x.id);
                    table.ForeignKey(
                        name: "fk_loot_box_group_game_game_id",
                        column: x => x.game_id,
                        principalTable: "Game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_loot_box_group_group_loot_box_group_id",
                        column: x => x.group_id,
                        principalTable: "GroupLootBox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_loot_box_group_loot_box_box_id",
                        column: x => x.box_id,
                        principalTable: "LootBox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LootBoxInventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
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

            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("aa4ba3c3-f6ac-42eb-b0ed-ec50cd65a792"), "dota2" },
                    { new Guid("cbd9f117-1cd1-47ad-a1e2-d1af22899d86"), "csgo" }
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
                name: "ix_game_item_quality_id",
                table: "GameItem",
                column: "quality_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_item_rarity_id",
                table: "GameItem",
                column: "rarity_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_item_type_id",
                table: "GameItem",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_item_quality_id1",
                table: "GameItemQuality",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_quality_name",
                table: "GameItemQuality",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_rarity_id1",
                table: "GameItemRarity",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_rarity_name",
                table: "GameItemRarity",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_type_id1",
                table: "GameItemType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_type_name",
                table: "GameItemType",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_group_loot_box_id",
                table: "GroupLootBox",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_group_loot_box_name",
                table: "GroupLootBox",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_game_id",
                table: "LootBox",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_id",
                table: "LootBox",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_name",
                table: "LootBox",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_banner_box_id",
                table: "LootBoxBanner",
                column: "box_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_banner_id",
                table: "LootBoxBanner",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_group_box_id",
                table: "LootBoxGroup",
                column: "box_id");

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_group_game_id",
                table: "LootBoxGroup",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_group_group_id",
                table: "LootBoxGroup",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_loot_box_group_id",
                table: "LootBoxGroup",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LootBoxBanner");

            migrationBuilder.DropTable(
                name: "LootBoxGroup");

            migrationBuilder.DropTable(
                name: "LootBoxInventory");

            migrationBuilder.DropTable(
                name: "GroupLootBox");

            migrationBuilder.DropTable(
                name: "GameItem");

            migrationBuilder.DropTable(
                name: "LootBox");

            migrationBuilder.DropTable(
                name: "GameItemQuality");

            migrationBuilder.DropTable(
                name: "GameItemRarity");

            migrationBuilder.DropTable(
                name: "GameItemType");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
