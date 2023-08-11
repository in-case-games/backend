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
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                    is_locked = table.Column<bool>(type: "boolean", nullable: false),
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
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rarity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quality_id = table.Column<Guid>(type: "uuid", nullable: false)
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
                        name: "fk_game_item_item_types_type_id",
                        column: x => x.type_id,
                        principalTable: "GameItemType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_item_qualities_quality_id",
                        column: x => x.quality_id,
                        principalTable: "GameItemQuality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_item_rarities_rarity_id",
                        column: x => x.rarity_id,
                        principalTable: "GameItemRarity",
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
                    { new Guid("18402de5-57f8-46ca-a5a3-571a9f83ee48"), "dota2" },
                    { new Guid("76ad9e8b-067c-498c-9e51-65b76346560c"), "csgo" }
                });

            migrationBuilder.InsertData(
                table: "GameItemQuality",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0e2c25a4-5dde-4010-ab1b-6276fa33e498"), "minimal wear" },
                    { new Guid("1613f962-b112-48f4-bca2-6ca863399249"), "well worn" },
                    { new Guid("229620b1-3d61-47f0-bb84-1ece8803ebea"), "battle scarred" },
                    { new Guid("9622ad13-cf97-4456-9e07-4311886cb877"), "factory new" },
                    { new Guid("9da9936a-cb19-4a14-9ecc-1d25c237cdb4"), "none" },
                    { new Guid("c0824aec-72bb-4eb3-96b8-c724e27cf2ff"), "field tested" }
                });

            migrationBuilder.InsertData(
                table: "GameItemRarity",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("240020d5-b056-41c7-b216-255baa5eee8d"), "red" },
                    { new Guid("a0343a73-5243-4b8e-a064-0545a79fcb9d"), "pink" },
                    { new Guid("ab36cc66-e281-4d0f-8e33-660e6429825d"), "violet" },
                    { new Guid("eb246068-b99d-47ff-aeb2-66e679c91c6b"), "gold" },
                    { new Guid("ec9fdce7-1463-4204-9890-55b82b912419"), "white" },
                    { new Guid("f762ae3c-e24f-4a24-b929-a23a4e8d6b86"), "blue" }
                });

            migrationBuilder.InsertData(
                table: "GameItemType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("6b160069-bd87-4c42-aff1-ee69d52082b0"), "weapon" },
                    { new Guid("944ec046-e4f1-47d3-8274-9a844cb667ab"), "other" },
                    { new Guid("9b013058-3a93-43d5-a3a5-1a7b18351498"), "none" },
                    { new Guid("c4961b96-1867-4478-9845-41bd2af9cdae"), "gloves" },
                    { new Guid("c673e721-72cd-47f8-994f-a168893dcdd2"), "knife" },
                    { new Guid("ce512efe-c4b9-4cb4-b6bc-6b6b9b4e5617"), "pistol" },
                    { new Guid("f92a1e42-7b73-44d9-8046-322245716a68"), "rifle" }
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
                name: "GameItemType");

            migrationBuilder.DropTable(
                name: "GameItemQuality");

            migrationBuilder.DropTable(
                name: "GameItemRarity");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
