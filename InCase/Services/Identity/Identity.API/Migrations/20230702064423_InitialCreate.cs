using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestrictionType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_restriction_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserRestriction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: true),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_restriction_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionalInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_uri = table.Column<string>(type: "text", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_additional_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_additional_info_roles_role_id",
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

            migrationBuilder.InsertData(
                table: "RestrictionType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("5e5ce500-238c-4ef1-9d14-ca49362a7192"), "mute" },
                    { new Guid("902a10b7-767d-4709-b4aa-8d40ff71104a"), "ban" },
                    { new Guid("d72b8ed8-eafa-44b5-9804-b3d3ad5b44b0"), "warn" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1683f486-6f43-4956-bdcf-5d71339ef054"), "admin" },
                    { new Guid("6d524965-fc21-4784-a0e1-f211b20740c5"), "bot" },
                    { new Guid("900a3e0c-7c61-4042-8dff-3085dad06e9f"), "owner" },
                    { new Guid("a4c7537d-fd82-43b7-9ebe-a9c13fad8347"), "user" }
                });

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
                name: "UserAdditionalInfo");

            migrationBuilder.DropTable(
                name: "UserRestriction");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "RestrictionType");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
