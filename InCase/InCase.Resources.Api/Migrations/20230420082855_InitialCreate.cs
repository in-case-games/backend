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
                    domain_uri = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    image_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_history_payment", x => x.id);
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
                name: "UserHistoryWithdrawn",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_history_withdrawn", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_history_withdrawn_game_item_item_id",
                        column: x => x.item_id,
                        principalTable: "GameItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_history_withdrawn_user_user_id",
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
                    { new Guid("54109d86-7cf6-4104-a184-dcff10e04234"), "dota" },
                    { new Guid("abc543b9-95b7-4dc0-99e0-034ec6c7a36f"), "csgo" },
                    { new Guid("f3a487fe-2230-45a6-b555-bfb5e2e9d35b"), "genshin" }
                });

            migrationBuilder.InsertData(
                table: "GameItemQuality",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("091e9a99-a790-45ea-a622-1e6535e20cc2"), "battle scarred" },
                    { new Guid("25a3f9ce-db6e-44c4-ae24-95327735fc74"), "minimal wear" },
                    { new Guid("794bf1c8-0cd5-4099-84a2-e7daf9b98110"), "well worn" },
                    { new Guid("996983e9-51e6-4019-910c-c0b8d5c00a45"), "field tested" },
                    { new Guid("9aaba4f0-8a8f-4dba-b7f0-d52ab30e528e"), "none" },
                    { new Guid("e97280dc-6fef-4e2c-8d97-e495ec5a0fc5"), "factory new" }
                });

            migrationBuilder.InsertData(
                table: "GameItemRarity",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("398f8598-a16a-43d4-9c47-acae3b7ce165"), "blue" },
                    { new Guid("90913418-8b45-4a31-962c-13b63a2375c6"), "pink" },
                    { new Guid("99507c39-c1d5-4add-ac7b-0e412fb21533"), "violet" },
                    { new Guid("a8d3e6eb-aaa0-403a-9699-44ff0038bab2"), "white" },
                    { new Guid("bbac2913-e99c-4827-927c-b2aaf4f6b98d"), "red" },
                    { new Guid("cb6951cb-eb95-475a-9ea2-dbaf2be98a55"), "gold" }
                });

            migrationBuilder.InsertData(
                table: "GameItemType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("082ecfdd-0c08-4a7c-be5f-15e19ee075a4"), "pistol" },
                    { new Guid("3b1157fb-02f1-4e4e-a645-96074224bd6b"), "gloves" },
                    { new Guid("6c168a46-bbc1-4873-b14b-58bb157598de"), "weapon" },
                    { new Guid("7beb83d9-4eb2-4fe3-a2ea-8867b579f616"), "knife" },
                    { new Guid("7bf0193a-cc55-43e1-acb6-fe7ca14b839a"), "none" },
                    { new Guid("8bab07db-373f-4380-9b8a-4de1d88f9e8e"), "rifle" }
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("20328955-2c0f-48cb-96e8-eb66a32c2906"), "case" },
                    { new Guid("3f2ce8cb-f63c-46bb-99a7-fba36332ab08"), "balance" }
                });

            migrationBuilder.InsertData(
                table: "RestrictionType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0e136dbf-d615-4c93-b9b4-0a547a8b9969"), "ban" },
                    { new Guid("6fb991ec-5849-4ec9-8424-10838c2b05fb"), "warn" },
                    { new Guid("bbbdc147-e4a2-4596-9d4d-cb974fa49443"), "mute" }
                });

            migrationBuilder.InsertData(
                table: "SiteStatistics",
                columns: new[] { "id", "loot_boxes", "reviews", "users", "withdrawn_funds", "withdrawn_items" },
                values: new object[] { new Guid("a7d1de31-cfe9-4820-9d7b-8d9225ef5714"), 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "SiteStatisticsAdmin",
                columns: new[] { "id", "balance_withdrawn", "sent_sites", "total_replenished" },
                values: new object[] { new Guid("a4101455-30c0-4d9a-a405-fa6e9f1e2626"), 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("34e51006-214f-41d8-8c53-d02cc60fb007"), "bot" },
                    { new Guid("5659998f-0e86-4380-a777-ad72b7cf57ae"), "admin" },
                    { new Guid("8258005c-1b2c-4e8b-a072-a9f352035722"), "user" },
                    { new Guid("9a24d04b-2ec5-4733-a715-931911a75b61"), "owner" }
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
                name: "ix_game_market_domain_uri",
                table: "GameMarket",
                column: "domain_uri",
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
                name: "ix_user_history_withdrawn_id",
                table: "UserHistoryWithdrawn",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdrawn_item_id",
                table: "UserHistoryWithdrawn",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_history_withdrawn_user_id",
                table: "UserHistoryWithdrawn",
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
                name: "GameMarket");

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
                name: "UserHistoryWithdrawn");

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
                name: "Promocode");

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
