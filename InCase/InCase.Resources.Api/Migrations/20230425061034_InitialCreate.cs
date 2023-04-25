using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InCase.Resources.Api.Migrations
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemQuality",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item_quality", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemRarity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item_rarity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_item_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupLootBox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_loot_box", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePaymentStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_payment_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ItemWithdrawStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_withdraw_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PromocodeType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promocode_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RestrictionType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_restriction_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SiteStatistics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    users = table.Column<int>(type: "int", nullable: false),
                    reviews = table.Column<int>(type: "int", nullable: false),
                    loot_boxes = table.Column<int>(type: "int", nullable: false),
                    withdrawn_items = table.Column<int>(type: "int", nullable: false),
                    withdrawn_funds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_site_statistics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SiteStatisticsAdmin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    balance_withdrawn = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    total_replenished = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    sent_sites = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_site_statistics_admin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    password_salt = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameMarket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "LootBox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    balance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    virtual_balance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_locked = table.Column<bool>(type: "bit", nullable: false),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loot_box", x => x.id);
                    table.ForeignKey(
                        name: "fk_loot_box_game_game_id",
                        column: x => x.game_id,
                        principalTable: "Game",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GameItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hash_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_for_market = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    rarity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    quality_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        name: "fk_game_item_game_item_qualities_quality_id",
                        column: x => x.quality_id,
                        principalTable: "GameItemQuality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_game_item_game_item_rarities_rarity_id",
                        column: x => x.rarity_id,
                        principalTable: "GameItemRarity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_game_item_game_item_types_type_id",
                        column: x => x.type_id,
                        principalTable: "GameItemType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Promocode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    discount = table.Column<decimal>(type: "DECIMAL(5,5)", nullable: false),
                    number_activations = table.Column<int>(type: "int", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promocode", x => x.id);
                    table.ForeignKey(
                        name: "fk_promocode_promocode_types_type_id",
                        column: x => x.type_id,
                        principalTable: "PromocodeType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportTopic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    content = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_topic", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_topic_users_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryPayment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    rate = table.Column<decimal>(type: "DECIMAL(6,5)", nullable: false),
                    status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_history_payment", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_history_payment_invoice_payment_status_status_id",
                        column: x => x.status_id,
                        principalTable: "InvoicePaymentStatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_history_payment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRestriction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    owner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_restriction", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_restriction_restriction_type_type_id",
                        column: x => x.type_id,
                        principalTable: "RestrictionType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_restriction_user_owner_id",
                        column: x => x.owner_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_user_restriction_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserReview",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    content = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    score = table.Column<int>(type: "int", nullable: false),
                    is_approved = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_review", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_review_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionalInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    balance = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_notify_email = table.Column<bool>(type: "bit", nullable: false),
                    is_guest_mode = table.Column<bool>(type: "bit", nullable: false),
                    is_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_additional_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_additional_info_user_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "UserRole",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_additional_info_users_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LootBoxBanner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    box_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    box_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    chance_wining = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    box_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "UserHistoryOpening",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    box_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_history_opening", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_history_opening_game_item_item_id",
                        column: x => x.item_id,
                        principalTable: "GameItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_history_opening_loot_box_box_id",
                        column: x => x.box_id,
                        principalTable: "LootBox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_history_opening_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryWithdraw",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_for_market = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    fixed_cost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    market_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_history_withdraw_item_withdraw_status_status_id",
                        column: x => x.status_id,
                        principalTable: "ItemWithdrawStatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_history_withdraw_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fixed_cost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "UserHistoryPromocode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_activated = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    promocode_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_history_promocode", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_history_promocode_promocode_promocode_id",
                        column: x => x.promocode_id,
                        principalTable: "Promocode",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_history_promocode_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportTopicAnswer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    plaintiff_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    topic_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_topic_answer", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_topic_answer_support_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "SupportTopic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_topic_answer_users_plaintiff_id",
                        column: x => x.plaintiff_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    review_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_review_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_review_image_user_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "UserReview",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPathBanner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    number_steps = table.Column<int>(type: "int", nullable: false),
                    fixed_cost = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    banner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        name: "fk_user_path_banner_loot_box_banner_banner_id",
                        column: x => x.banner_id,
                        principalTable: "LootBoxBanner",
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
                name: "AnswerImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answer_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_answer_image_support_topic_answers_answer_id",
                        column: x => x.answer_id,
                        principalTable: "SupportTopicAnswer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("b66c1005-7d29-430d-a173-f56935ab12e9"), "csgo" },
                    { new Guid("cdca2978-ef58-43ce-9c02-346e94e14822"), "dota2" },
                    { new Guid("e2274b22-a386-4d0e-a50a-4acca332ba27"), "genshin" }
                });

            migrationBuilder.InsertData(
                table: "GameItemQuality",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0618a777-8f64-4c6b-8b58-49fccadedca6"), "minimal wear" },
                    { new Guid("0d884ed4-0f21-4a84-a54a-0cb84818a493"), "battle scarred" },
                    { new Guid("0e993d3c-801d-4012-8b17-0ce048069090"), "well worn" },
                    { new Guid("241952a0-ab30-4b32-bf78-d551e47482e3"), "field tested" },
                    { new Guid("65444871-dbe0-406b-a5de-9afd04594683"), "factory new" },
                    { new Guid("7af96858-bc67-4a90-98f9-d80e8f4bfd39"), "none" }
                });

            migrationBuilder.InsertData(
                table: "GameItemRarity",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("443aa00f-c673-460d-886a-df0c78acfa49"), "pink" },
                    { new Guid("5877f56c-50da-4200-8b0a-eddb6bd1e6b0"), "red" },
                    { new Guid("5a2481c2-ae89-40b3-b80d-b85cf137a2b5"), "blue" },
                    { new Guid("8fc091f9-781b-4081-a4a4-9c3f0ccb6c05"), "violet" },
                    { new Guid("dfef8fd9-bf06-404c-ae7a-44fd4c9c7324"), "white" },
                    { new Guid("f87d9308-4405-46e5-951c-0ce35c4e0f77"), "gold" }
                });

            migrationBuilder.InsertData(
                table: "GameItemType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("022891b0-9749-44d4-b7c0-075116c60257"), "knife" },
                    { new Guid("59b77377-fd48-4bd5-afac-1c67cd808de5"), "gloves" },
                    { new Guid("6050dd1e-6709-44c2-a810-300c1a5c25d1"), "pistol" },
                    { new Guid("8f344694-8ab1-4bd5-a8cb-dde3bfb58869"), "rifle" },
                    { new Guid("ee69920e-eddf-4807-bba7-f90ccd2b12dc"), "none" },
                    { new Guid("f9c5f9c4-8823-46b1-aec8-545966c42f80"), "weapon" }
                });

            migrationBuilder.InsertData(
                table: "InvoicePaymentStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0f005649-660a-4fbc-b55f-b6eedbe76a14"), "processing" },
                    { new Guid("1164c506-d481-4c7a-8ac8-3da8808fae8e"), "paid" },
                    { new Guid("3a03165a-b060-458c-a6e3-a62fe5ec7482"), "refund" },
                    { new Guid("6b2e933b-4a1c-41b2-baf6-fb86fa62dfaf"), "refused" },
                    { new Guid("b30412f4-a415-481f-81b0-154a116544f9"), "chargeback" },
                    { new Guid("c4d6d89d-7b54-4029-80b0-042e422801fb"), "new" },
                    { new Guid("faec0fec-b23b-42a8-a9df-3ce9a30b3460"), "chargeback-cancel" }
                });

            migrationBuilder.InsertData(
                table: "ItemWithdrawStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("335e1d4b-ea74-4bfd-ad90-3922ba29b123"), "waiting" },
                    { new Guid("6ae5f47c-7c03-4460-ba1d-7d2d4c1836aa"), "transfer" },
                    { new Guid("b27b3448-4c6b-48cb-a77e-bced87bfdbfb"), "given" },
                    { new Guid("efe8c591-b35c-4a9d-ab05-4b850566b911"), "purchase" },
                    { new Guid("f17a134a-2bd1-4ae4-9c80-bcb7ce775744"), "cancel" }
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("5debea17-f77f-40f0-ac28-1dfc7dfde59d"), "balance" },
                    { new Guid("b7f20e02-fa3c-44ca-b6c9-1a1d5b4c6de0"), "case" }
                });

            migrationBuilder.InsertData(
                table: "RestrictionType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("4c532a85-997b-405c-b9d0-eee4845e6fef"), "warn" },
                    { new Guid("92081c88-ede8-4999-83c0-83c21440f072"), "mute" },
                    { new Guid("eb6e6de8-6fc8-44ce-8dc3-231136dcda49"), "ban" }
                });

            migrationBuilder.InsertData(
                table: "SiteStatistics",
                columns: new[] { "id", "loot_boxes", "reviews", "users", "withdrawn_funds", "withdrawn_items" },
                values: new object[] { new Guid("a49cabdd-f292-42ec-abbe-6e500bc098eb"), 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "SiteStatisticsAdmin",
                columns: new[] { "id", "balance_withdrawn", "sent_sites", "total_replenished" },
                values: new object[] { new Guid("874022d2-dae9-4bb7-b19d-d52707aac686"), 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("559d9b8c-13f7-4ab8-86a2-c4574852829c"), "user" },
                    { new Guid("7f49dad8-5849-4b3a-8062-f00aed6dd267"), "bot" },
                    { new Guid("b4e6fe1c-5ee0-4434-b421-0efcdb8767ba"), "admin" },
                    { new Guid("ff5ed718-8e47-4147-ac45-edffec726072"), "owner" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_answer_image_answer_id",
                table: "AnswerImage",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "ix_answer_image_id",
                table: "AnswerImage",
                column: "id",
                unique: true);

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
                name: "ix_game_item_quality_id",
                table: "GameItemQuality",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_quality_name",
                table: "GameItemQuality",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_rarity_id",
                table: "GameItemRarity",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_rarity_name",
                table: "GameItemRarity",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_type_id",
                table: "GameItemType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_item_type_name",
                table: "GameItemType",
                column: "name",
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
                name: "ix_invoice_payment_status_id",
                table: "InvoicePaymentStatus",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoice_payment_status_name",
                table: "InvoicePaymentStatus",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_item_withdraw_status_id",
                table: "ItemWithdrawStatus",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_item_withdraw_status_name",
                table: "ItemWithdrawStatus",
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

            migrationBuilder.CreateIndex(
                name: "ix_promocode_id",
                table: "Promocode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promocode_name",
                table: "Promocode",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promocode_type_id",
                table: "Promocode",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "ix_promocode_type_id",
                table: "PromocodeType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promocode_type_name",
                table: "PromocodeType",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_restriction_type_id",
                table: "RestrictionType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_restriction_type_name",
                table: "RestrictionType",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_review_image_id",
                table: "ReviewImage",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_review_image_review_id",
                table: "ReviewImage",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "ix_site_statistics_id",
                table: "SiteStatistics",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_site_statistics_admin_id",
                table: "SiteStatisticsAdmin",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_support_topic_id",
                table: "SupportTopic",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_support_topic_user_id",
                table: "SupportTopic",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_topic_answer_id",
                table: "SupportTopicAnswer",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_support_topic_answer_plaintiff_id",
                table: "SupportTopicAnswer",
                column: "plaintiff_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_topic_answer_topic_id",
                table: "SupportTopicAnswer",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_email",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_login",
                table: "User",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_additional_info_id",
                table: "UserAdditionalInfo",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_additional_info_role_id",
                table: "UserAdditionalInfo",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_additional_info_user_id",
                table: "UserAdditionalInfo",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_opening_box_id",
                table: "UserHistoryOpening",
                column: "box_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_opening_id",
                table: "UserHistoryOpening",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_opening_item_id",
                table: "UserHistoryOpening",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_opening_user_id",
                table: "UserHistoryOpening",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_payment_id",
                table: "UserHistoryPayment",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_payment_status_id",
                table: "UserHistoryPayment",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_payment_user_id",
                table: "UserHistoryPayment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_promocode_id",
                table: "UserHistoryPromocode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_promocode_promocode_id",
                table: "UserHistoryPromocode",
                column: "promocode_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_promocode_user_id",
                table: "UserHistoryPromocode",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_id",
                table: "UserHistoryWithdraw",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdraw_id_for_market",
                table: "UserHistoryWithdraw",
                column: "id_for_market",
                unique: true);

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
                name: "ix_user_path_banner_banner_id",
                table: "UserPathBanner",
                column: "banner_id");

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
                name: "ix_user_restriction_id",
                table: "UserRestriction",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_restriction_owner_id",
                table: "UserRestriction",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_restriction_type_id",
                table: "UserRestriction",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_restriction_user_id",
                table: "UserRestriction",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_review_id",
                table: "UserReview",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_review_user_id",
                table: "UserReview",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_id",
                table: "UserRole",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_role_name",
                table: "UserRole",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerImage");

            migrationBuilder.DropTable(
                name: "LootBoxGroup");

            migrationBuilder.DropTable(
                name: "LootBoxInventory");

            migrationBuilder.DropTable(
                name: "ReviewImage");

            migrationBuilder.DropTable(
                name: "SiteStatistics");

            migrationBuilder.DropTable(
                name: "SiteStatisticsAdmin");

            migrationBuilder.DropTable(
                name: "UserAdditionalInfo");

            migrationBuilder.DropTable(
                name: "UserHistoryOpening");

            migrationBuilder.DropTable(
                name: "UserHistoryPayment");

            migrationBuilder.DropTable(
                name: "UserHistoryPromocode");

            migrationBuilder.DropTable(
                name: "UserHistoryWithdraw");

            migrationBuilder.DropTable(
                name: "UserInventory");

            migrationBuilder.DropTable(
                name: "UserPathBanner");

            migrationBuilder.DropTable(
                name: "UserRestriction");

            migrationBuilder.DropTable(
                name: "SupportTopicAnswer");

            migrationBuilder.DropTable(
                name: "GroupLootBox");

            migrationBuilder.DropTable(
                name: "UserReview");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "InvoicePaymentStatus");

            migrationBuilder.DropTable(
                name: "Promocode");

            migrationBuilder.DropTable(
                name: "GameMarket");

            migrationBuilder.DropTable(
                name: "ItemWithdrawStatus");

            migrationBuilder.DropTable(
                name: "GameItem");

            migrationBuilder.DropTable(
                name: "LootBoxBanner");

            migrationBuilder.DropTable(
                name: "RestrictionType");

            migrationBuilder.DropTable(
                name: "SupportTopic");

            migrationBuilder.DropTable(
                name: "PromocodeType");

            migrationBuilder.DropTable(
                name: "GameItemQuality");

            migrationBuilder.DropTable(
                name: "GameItemRarity");

            migrationBuilder.DropTable(
                name: "GameItemType");

            migrationBuilder.DropTable(
                name: "LootBox");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
