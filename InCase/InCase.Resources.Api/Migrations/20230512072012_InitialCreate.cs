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
                    { new Guid("38522867-fd2c-4494-8410-a651e43b53ed"), "dota2" },
                    { new Guid("516b157f-c6eb-4558-9465-b195f8604ebc"), "csgo" }
                });

            migrationBuilder.InsertData(
                table: "GameItemQuality",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2bb9b883-e665-4c1e-b724-8c473164e656"), "minimal wear" },
                    { new Guid("3e4d9ff4-e95a-463d-b025-428750f95049"), "factory new" },
                    { new Guid("4ff00602-1ad7-442f-89a9-5af621800a7e"), "field tested" },
                    { new Guid("5f5d4ebc-9fc3-4cba-b9f0-48a25db1b478"), "battle scarred" },
                    { new Guid("7f332074-3a1d-473c-a385-f8ebe457e214"), "none" },
                    { new Guid("aaebce7b-8cf0-4bed-8d81-f3dcebd06576"), "well worn" }
                });

            migrationBuilder.InsertData(
                table: "GameItemRarity",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2e840227-2d98-440b-b172-0b51af234aef"), "red" },
                    { new Guid("87d4d091-97af-454e-9661-9cc211674230"), "gold" },
                    { new Guid("d115030f-0665-4bde-86ce-5bc107cd9812"), "white" },
                    { new Guid("d8bb4fda-0c3b-4a13-b976-02de3f1b67a8"), "blue" },
                    { new Guid("e0981c89-97a9-4691-820c-eb5609624bdf"), "violet" },
                    { new Guid("eb53198f-cb48-4878-8db9-4e2890d5dd27"), "pink" }
                });

            migrationBuilder.InsertData(
                table: "GameItemType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0b79f499-8b62-4856-adf7-9ef590e5c83c"), "pistol" },
                    { new Guid("19eee306-f0f2-4707-85c0-aabe4ab2583d"), "knife" },
                    { new Guid("723ddf43-b95e-4c39-b5b1-738a355c8488"), "other" },
                    { new Guid("8004b91e-eb09-4d37-904d-258265fb5aba"), "gloves" },
                    { new Guid("dcae4d9e-6cb3-453d-8732-f5688dd63b8c"), "rifle" },
                    { new Guid("e3a0e7f7-4d31-42c0-883a-25c4a27e55ad"), "weapon" },
                    { new Guid("f0ade5cb-c88c-46bd-976e-92a23f9a3b73"), "none" }
                });

            migrationBuilder.InsertData(
                table: "InvoicePaymentStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1794848e-7e65-499f-93d8-d7c751ca37fc"), "chargeback" },
                    { new Guid("35b30834-6e4f-462f-9e86-bffb9fb35431"), "refused" },
                    { new Guid("53c9a73f-a347-46de-89db-2041a0d19623"), "chargeback-cancel" },
                    { new Guid("69df76ab-6959-48e4-880c-4649935a750d"), "new" },
                    { new Guid("9d5ca22f-5011-409a-bba7-b33870f38f78"), "paid" },
                    { new Guid("edd10e79-3263-4b18-a578-3675db502f34"), "processing" },
                    { new Guid("f68e75de-5516-423d-a888-0b981f3bad45"), "refund" }
                });

            migrationBuilder.InsertData(
                table: "ItemWithdrawStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1c7a2e86-9997-4efb-a729-2ebbcdc7bdac"), "purchase" },
                    { new Guid("27b3be39-de0e-42a7-94ae-7e9599f4271e"), "transfer" },
                    { new Guid("7ae2c780-911a-452f-8aec-275bbce4a8cb"), "cancel" },
                    { new Guid("a3b139ce-aab9-4d23-b7ec-5eb7daf936e9"), "given" },
                    { new Guid("e6bd2afc-cc0c-47d3-bcb7-1b2bfb49bbe4"), "waiting" }
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("17434bcd-c65c-4a7b-9eb9-a28fc3890d19"), "balance" },
                    { new Guid("5c25e4c4-d5d8-459e-881d-6c23545d4910"), "case" }
                });

            migrationBuilder.InsertData(
                table: "RestrictionType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0ba51a18-4951-4f9d-93f2-1ca8c52f44c5"), "warn" },
                    { new Guid("488c099d-3c44-440f-b40d-4d6e0e5460a5"), "ban" },
                    { new Guid("4e10c704-9ed4-455d-ae24-aee39441fb4f"), "mute" }
                });

            migrationBuilder.InsertData(
                table: "SiteStatistics",
                columns: new[] { "id", "loot_boxes", "reviews", "users", "withdrawn_funds", "withdrawn_items" },
                values: new object[] { new Guid("77122740-17cc-4c88-8f05-c52fd732ec28"), 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "SiteStatisticsAdmin",
                columns: new[] { "id", "balance_withdrawn", "sent_sites", "total_replenished" },
                values: new object[] { new Guid("1673210c-f32f-49d9-b253-534e215940dd"), 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("66f419f6-4a30-41eb-9794-93b68bc56dac"), "admin" },
                    { new Guid("6a820d04-4d63-4a85-9ad9-7c183f133aab"), "user" },
                    { new Guid("b3f2d3ca-fd04-4bf7-9637-d5deea4dd3b2"), "owner" },
                    { new Guid("f051e001-a877-4d9d-a641-6b115c6d43b4"), "bot" }
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
