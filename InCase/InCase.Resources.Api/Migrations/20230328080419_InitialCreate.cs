using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    name = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameitemquality", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemRarity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameitemrarity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameItemType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameitemtype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupLootBox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grouplootbox", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promocodetype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RestrictionType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_restrictiontype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SiteStatitics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    users = table.Column<int>(type: "int", nullable: false),
                    reviews = table.Column<int>(type: "int", nullable: false),
                    opencases = table.Column<int>(type: "int", nullable: false),
                    withdrawnitems = table.Column<int>(type: "int", nullable: false),
                    withdrawnfunds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sitestatitics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SiteStatiticsAdmin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    balancewithdrawn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalreplenished = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sentsites = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sitestatiticsadmin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    login = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    passwordhash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordsalt = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userrole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    domainuri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gameid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameplatform", x => x.id);
                    table.ForeignKey(
                        name: "fk_gameplatform_game_gameid",
                        column: x => x.gameid,
                        principalTable: "Game",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LootBox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    virtualbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    islocked = table.Column<bool>(type: "bit", nullable: false),
                    gameid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lootbox", x => x.id);
                    table.ForeignKey(
                        name: "fk_lootbox_game_gameid",
                        column: x => x.gameid,
                        principalTable: "Game",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GameItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idforplatform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gameid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    typeid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    rarityid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    qualityid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameitem", x => x.id);
                    table.ForeignKey(
                        name: "fk_gameitem_game_gameid",
                        column: x => x.gameid,
                        principalTable: "Game",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_gameitem_gameitemqualities_qualityid",
                        column: x => x.qualityid,
                        principalTable: "GameItemQuality",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_gameitem_gameitemrarities_rarityid",
                        column: x => x.rarityid,
                        principalTable: "GameItemRarity",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_gameitem_gameitemtypes_typeid",
                        column: x => x.typeid,
                        principalTable: "GameItemType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "NewsImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    newsid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_newsimage", x => x.id);
                    table.ForeignKey(
                        name: "fk_newsimage_news_newsid",
                        column: x => x.newsid,
                        principalTable: "News",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Promocode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    discount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numberactivations = table.Column<int>(type: "int", nullable: false),
                    expirationdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    typeid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promocode", x => x.id);
                    table.ForeignKey(
                        name: "fk_promocode_promocodetypes_typeid",
                        column: x => x.typeid,
                        principalTable: "PromocodeType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SupportTopic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isclosed = table.Column<bool>(type: "bit", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    supportid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supporttopic", x => x.id);
                    table.ForeignKey(
                        name: "fk_supporttopic_users_supportid",
                        column: x => x.supportid,
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_supporttopic_users_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryPayment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userhistorypayment", x => x.id);
                    table.ForeignKey(
                        name: "fk_userhistorypayment_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserRestriction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creationdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expirationdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ownerid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    typeid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userrestriction", x => x.id);
                    table.ForeignKey(
                        name: "fk_userrestriction_restrictiontype_typeid",
                        column: x => x.typeid,
                        principalTable: "RestrictionType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userrestriction_user_ownerid",
                        column: x => x.ownerid,
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userrestriction_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserReview",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isapproved = table.Column<bool>(type: "bit", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userreview", x => x.id);
                    table.ForeignKey(
                        name: "fk_userreview_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    refresh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ipaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    device = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usertoken", x => x.id);
                    table.ForeignKey(
                        name: "fk_usertoken_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionalInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isnotifyemail = table.Column<bool>(type: "bit", nullable: false),
                    isguestmode = table.Column<bool>(type: "bit", nullable: false),
                    roleid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_useradditionalinfo", x => x.id);
                    table.ForeignKey(
                        name: "fk_useradditionalinfo_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_useradditionalinfo_userroles_roleid",
                        column: x => x.roleid,
                        principalTable: "UserRole",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LootBoxBanner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    creationdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expirationdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    boxid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lootboxbanner", x => x.id);
                    table.ForeignKey(
                        name: "fk_lootboxbanner_lootbox_boxid",
                        column: x => x.boxid,
                        principalTable: "LootBox",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LootBoxGroup",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    boxid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    groupid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    gameid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lootboxgroup", x => x.id);
                    table.ForeignKey(
                        name: "fk_lootboxgroup_game_gameid",
                        column: x => x.gameid,
                        principalTable: "Game",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_lootboxgroup_grouplootbox_groupid",
                        column: x => x.groupid,
                        principalTable: "GroupLootBox",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_lootboxgroup_lootbox_boxid",
                        column: x => x.boxid,
                        principalTable: "LootBox",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LootBoxInventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    numberitems = table.Column<int>(type: "int", nullable: false),
                    chancewining = table.Column<int>(type: "int", nullable: false),
                    itemid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    boxid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lootboxinventory", x => x.id);
                    table.ForeignKey(
                        name: "fk_lootboxinventory_gameitem_itemid",
                        column: x => x.itemid,
                        principalTable: "GameItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_lootboxinventory_lootbox_boxid",
                        column: x => x.boxid,
                        principalTable: "LootBox",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryOpening",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    boxid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userhistoryopening", x => x.id);
                    table.ForeignKey(
                        name: "fk_userhistoryopening_gameitem_itemid",
                        column: x => x.itemid,
                        principalTable: "GameItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userhistoryopening_lootbox_boxid",
                        column: x => x.boxid,
                        principalTable: "LootBox",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userhistoryopening_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryWithdrawn",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userhistorywithdrawn", x => x.id);
                    table.ForeignKey(
                        name: "fk_userhistorywithdrawn_gameitem_itemid",
                        column: x => x.itemid,
                        principalTable: "GameItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userhistorywithdrawn_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserInventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fixedcost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userinventory", x => x.id);
                    table.ForeignKey(
                        name: "fk_userinventory_gameitem_itemid",
                        column: x => x.itemid,
                        principalTable: "GameItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userinventory_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryPromocode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isactivated = table.Column<bool>(type: "bit", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    promocodeid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userhistorypromocode", x => x.id);
                    table.ForeignKey(
                        name: "fk_userhistorypromocode_promocode_promocodeid",
                        column: x => x.promocodeid,
                        principalTable: "Promocode",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userhistorypromocode_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SupportTopicAnswer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    plaintiffid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    topicid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supporttopicanswer", x => x.id);
                    table.ForeignKey(
                        name: "fk_supporttopicanswer_supporttopic_topicid",
                        column: x => x.topicid,
                        principalTable: "SupportTopic",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_supporttopicanswer_users_plaintiffid",
                        column: x => x.plaintiffid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reviewid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviewimage", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviewimage_userreviews_reviewid",
                        column: x => x.reviewid,
                        principalTable: "UserReview",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserPathBanner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    numbersteps = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bannerid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userpathbanner", x => x.id);
                    table.ForeignKey(
                        name: "fk_userpathbanner_gameitem_itemid",
                        column: x => x.itemid,
                        principalTable: "GameItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userpathbanner_lootboxbanner_bannerid",
                        column: x => x.bannerid,
                        principalTable: "LootBoxBanner",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_userpathbanner_user_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AnswerImage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answerimage", x => x.id);
                    table.ForeignKey(
                        name: "fk_answerimage_supporttopicanswers_answerid",
                        column: x => x.answerid,
                        principalTable: "SupportTopicAnswer",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_answerimage_answerid",
                table: "AnswerImage",
                column: "answerid");

            migrationBuilder.CreateIndex(
                name: "ix_answerimage_id",
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
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_gameitem_gameid",
                table: "GameItem",
                column: "gameid");

            migrationBuilder.CreateIndex(
                name: "ix_gameitem_id",
                table: "GameItem",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gameitem_qualityid",
                table: "GameItem",
                column: "qualityid");

            migrationBuilder.CreateIndex(
                name: "ix_gameitem_rarityid",
                table: "GameItem",
                column: "rarityid");

            migrationBuilder.CreateIndex(
                name: "ix_gameitem_typeid",
                table: "GameItem",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "ix_gameitemquality_id",
                table: "GameItemQuality",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gameitemrarity_id",
                table: "GameItemRarity",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gameitemtype_id",
                table: "GameItemType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gameplatform_gameid",
                table: "GamePlatform",
                column: "gameid");

            migrationBuilder.CreateIndex(
                name: "ix_gameplatform_id",
                table: "GamePlatform",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_grouplootbox_id",
                table: "GroupLootBox",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_grouplootbox_name",
                table: "GroupLootBox",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_lootbox_gameid",
                table: "LootBox",
                column: "gameid");

            migrationBuilder.CreateIndex(
                name: "ix_lootbox_id",
                table: "LootBox",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lootboxbanner_boxid",
                table: "LootBoxBanner",
                column: "boxid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lootboxbanner_id",
                table: "LootBoxBanner",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lootboxgroup_boxid",
                table: "LootBoxGroup",
                column: "boxid");

            migrationBuilder.CreateIndex(
                name: "ix_lootboxgroup_gameid",
                table: "LootBoxGroup",
                column: "gameid");

            migrationBuilder.CreateIndex(
                name: "ix_lootboxgroup_groupid",
                table: "LootBoxGroup",
                column: "groupid");

            migrationBuilder.CreateIndex(
                name: "ix_lootboxgroup_id",
                table: "LootBoxGroup",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lootboxinventory_boxid",
                table: "LootBoxInventory",
                column: "boxid");

            migrationBuilder.CreateIndex(
                name: "ix_lootboxinventory_id",
                table: "LootBoxInventory",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lootboxinventory_itemid",
                table: "LootBoxInventory",
                column: "itemid");

            migrationBuilder.CreateIndex(
                name: "ix_news_id",
                table: "News",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_newsimage_id",
                table: "NewsImage",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_newsimage_newsid",
                table: "NewsImage",
                column: "newsid");

            migrationBuilder.CreateIndex(
                name: "ix_promocode_id",
                table: "Promocode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promocode_name",
                table: "Promocode",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_promocode_typeid",
                table: "Promocode",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "ix_promocodetype_id",
                table: "PromocodeType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promocodetype_name",
                table: "PromocodeType",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_restrictiontype_id",
                table: "RestrictionType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_restrictiontype_name",
                table: "RestrictionType",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_reviewimage_id",
                table: "ReviewImage",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reviewimage_reviewid",
                table: "ReviewImage",
                column: "reviewid");

            migrationBuilder.CreateIndex(
                name: "ix_sitestatitics_id",
                table: "SiteStatitics",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sitestatiticsadmin_id",
                table: "SiteStatiticsAdmin",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_supporttopic_id",
                table: "SupportTopic",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_supporttopic_supportid",
                table: "SupportTopic",
                column: "supportid");

            migrationBuilder.CreateIndex(
                name: "ix_supporttopic_userid",
                table: "SupportTopic",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_supporttopicanswer_id",
                table: "SupportTopicAnswer",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_supporttopicanswer_plaintiffid",
                table: "SupportTopicAnswer",
                column: "plaintiffid");

            migrationBuilder.CreateIndex(
                name: "ix_supporttopicanswer_topicid",
                table: "SupportTopicAnswer",
                column: "topicid");

            migrationBuilder.CreateIndex(
                name: "ix_user_email",
                table: "User",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_login",
                table: "User",
                column: "login",
                unique: true,
                filter: "[login] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_useradditionalinfo_id",
                table: "UserAdditionalInfo",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_useradditionalinfo_roleid",
                table: "UserAdditionalInfo",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "ix_useradditionalinfo_userid",
                table: "UserAdditionalInfo",
                column: "userid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userhistoryopening_boxid",
                table: "UserHistoryOpening",
                column: "boxid");

            migrationBuilder.CreateIndex(
                name: "ix_userhistoryopening_id",
                table: "UserHistoryOpening",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userhistoryopening_itemid",
                table: "UserHistoryOpening",
                column: "itemid");

            migrationBuilder.CreateIndex(
                name: "ix_userhistoryopening_userid",
                table: "UserHistoryOpening",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userhistorypayment_id",
                table: "UserHistoryPayment",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userhistorypayment_userid",
                table: "UserHistoryPayment",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userhistorypromocode_id",
                table: "UserHistoryPromocode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userhistorypromocode_promocodeid",
                table: "UserHistoryPromocode",
                column: "promocodeid");

            migrationBuilder.CreateIndex(
                name: "ix_userhistorypromocode_userid",
                table: "UserHistoryPromocode",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userhistorywithdrawn_id",
                table: "UserHistoryWithdrawn",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userhistorywithdrawn_itemid",
                table: "UserHistoryWithdrawn",
                column: "itemid");

            migrationBuilder.CreateIndex(
                name: "ix_userhistorywithdrawn_userid",
                table: "UserHistoryWithdrawn",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userinventory_id",
                table: "UserInventory",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userinventory_itemid",
                table: "UserInventory",
                column: "itemid");

            migrationBuilder.CreateIndex(
                name: "ix_userinventory_userid",
                table: "UserInventory",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userpathbanner_bannerid",
                table: "UserPathBanner",
                column: "bannerid");

            migrationBuilder.CreateIndex(
                name: "ix_userpathbanner_id",
                table: "UserPathBanner",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userpathbanner_itemid",
                table: "UserPathBanner",
                column: "itemid");

            migrationBuilder.CreateIndex(
                name: "ix_userpathbanner_userid",
                table: "UserPathBanner",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userrestriction_id",
                table: "UserRestriction",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userrestriction_ownerid",
                table: "UserRestriction",
                column: "ownerid");

            migrationBuilder.CreateIndex(
                name: "ix_userrestriction_typeid",
                table: "UserRestriction",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "ix_userrestriction_userid",
                table: "UserRestriction",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userreview_id",
                table: "UserReview",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userreview_userid",
                table: "UserReview",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_userrole_id",
                table: "UserRole",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userrole_name",
                table: "UserRole",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_usertoken_id",
                table: "UserToken",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usertoken_userid",
                table: "UserToken",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerImage");

            migrationBuilder.DropTable(
                name: "GamePlatform");

            migrationBuilder.DropTable(
                name: "LootBoxGroup");

            migrationBuilder.DropTable(
                name: "LootBoxInventory");

            migrationBuilder.DropTable(
                name: "NewsImage");

            migrationBuilder.DropTable(
                name: "ReviewImage");

            migrationBuilder.DropTable(
                name: "SiteStatitics");

            migrationBuilder.DropTable(
                name: "SiteStatiticsAdmin");

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
                name: "UserToken");

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
