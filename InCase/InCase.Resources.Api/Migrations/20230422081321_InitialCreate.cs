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
                name: "News",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news", x => x.id);
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
                name: "NewsImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    news_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_news_image_news_news_id",
                        column: x => x.news_id,
                        principalTable: "News",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    { new Guid("0ea6a922-9651-4946-a5a5-986a4388bd9b"), "genshin" },
                    { new Guid("1900c471-149a-4a39-b5df-402f5fb68571"), "dota2" },
                    { new Guid("4fe55ff2-1503-416b-8cee-3641afaa3336"), "csgo" }
                });

            migrationBuilder.InsertData(
                table: "GameItemQuality",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0c1f04d0-de6b-408d-96cb-71fd478abcf6"), "battle scarred" },
                    { new Guid("4113635a-86f1-48c9-808c-ae62123c8ac2"), "minimal wear" },
                    { new Guid("501eca94-fbc0-4d45-9429-5c609115fc14"), "well worn" },
                    { new Guid("61c67299-4834-4b5d-a325-041334288496"), "none" },
                    { new Guid("a9d34584-da18-4e32-9385-7293b2f4a157"), "field tested" },
                    { new Guid("e53c05a6-0b20-4572-aaa3-288f26655f88"), "factory new" }
                });

            migrationBuilder.InsertData(
                table: "GameItemRarity",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("28f30bf6-9d2f-49be-84ff-355b26e03f9e"), "pink" },
                    { new Guid("31838d28-6ff9-45bc-9c0f-3d650e3ba48b"), "blue" },
                    { new Guid("742450b1-66e8-4b00-aacb-5edfaec0b693"), "white" },
                    { new Guid("d15d7a75-abba-43a9-9479-7e3550ceb554"), "red" },
                    { new Guid("dc66ff67-1fcb-4823-becf-0a29f0cd3532"), "violet" },
                    { new Guid("fbbcbf6a-2b17-4afc-897d-fe4b3a3b4161"), "gold" }
                });

            migrationBuilder.InsertData(
                table: "GameItemType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("129e09cf-a193-4626-a3f5-92118051b437"), "gloves" },
                    { new Guid("1c891103-2070-496b-bf89-9feeae48aff1"), "knife" },
                    { new Guid("8e399e95-9d18-427c-aa9d-89215a9942e1"), "rifle" },
                    { new Guid("e17f1020-2c1d-4eab-8616-2a444033bd97"), "weapon" },
                    { new Guid("e6f85688-a201-489f-8c89-1c83a0378f44"), "pistol" },
                    { new Guid("f56f6b7a-d87c-42c5-98d9-e6f33a0e71af"), "none" }
                });

            migrationBuilder.InsertData(
                table: "InvoicePaymentStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("068c140e-eb24-4d60-83ce-83e7ad271de9"), "refused" },
                    { new Guid("427409a5-ecc1-4b74-b7d3-6f8c92179ca7"), "chargeback" },
                    { new Guid("5e9b2d77-0265-45a5-9ad9-5e9d87f0cb6d"), "refund" },
                    { new Guid("7cde74a5-db50-48ec-b546-852fcc99fd51"), "paid" },
                    { new Guid("80bd46ba-a169-456c-8ece-683ecdf120cd"), "chargeback-cancel" },
                    { new Guid("8aa72acd-b281-4922-85dd-5357e15a59b3"), "processing" },
                    { new Guid("8c441f99-50a9-4607-9b55-c23909acd9ff"), "new" }
                });

            migrationBuilder.InsertData(
                table: "ItemWithdrawStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1cb178fd-8f57-4987-a3a7-bd96d3421a04"), "cancel" },
                    { new Guid("4d45dbdc-ee95-4ee7-b017-87d96a76a327"), "transfer" },
                    { new Guid("9884b963-201e-4e3e-95b5-50d8662cea76"), "purchase" },
                    { new Guid("f034336a-276a-4f53-ab87-47c92ae1822c"), "waiting" },
                    { new Guid("fd601683-2d17-4290-9fd6-0f14c027fc82"), "given" }
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("451323af-0364-4537-820f-561919efaceb"), "case" },
                    { new Guid("ae142cfe-0694-4c16-abf3-74ee06076f54"), "balance" }
                });

            migrationBuilder.InsertData(
                table: "RestrictionType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("72873316-4c74-4d2b-9893-d7ad68dd8d95"), "mute" },
                    { new Guid("908201ab-704c-457b-8a83-a72531f1f5bd"), "warn" },
                    { new Guid("99813a33-6724-4fcb-93c2-21fa0d861457"), "ban" }
                });

            migrationBuilder.InsertData(
                table: "SiteStatistics",
                columns: new[] { "id", "loot_boxes", "reviews", "users", "withdrawn_funds", "withdrawn_items" },
                values: new object[] { new Guid("b75661cf-301a-4f1b-bb94-3aa0d3e2503d"), 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "SiteStatisticsAdmin",
                columns: new[] { "id", "balance_withdrawn", "sent_sites", "total_replenished" },
                values: new object[] { new Guid("3075322f-ffbf-441f-9d17-31f18b73e133"), 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2f9a160d-6a1f-45f2-b0e0-ea70f8555b2b"), "bot" },
                    { new Guid("afd073c3-533f-4b96-98cd-5c93f70b5d7e"), "owner" },
                    { new Guid("b33abdfa-be7b-41ee-b711-c9b69436f906"), "admin" },
                    { new Guid("d93c205b-053d-4617-a25c-b6b493ce15cc"), "user" }
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
                name: "ix_news_id",
                table: "News",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_news_image_id",
                table: "NewsImage",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_news_image_news_id",
                table: "NewsImage",
                column: "news_id");

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
                name: "NewsImage");

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
                name: "News");

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
