using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Promocode.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromoCodeType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promo_code_type", x => x.id);
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
                name: "PromoCode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    discount = table.Column<decimal>(type: "numeric(5,5)", nullable: false),
                    number_activations = table.Column<int>(type: "integer", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promo_code", x => x.id);
                    table.ForeignKey(
                        name: "fk_promo_code_promo_codes_types_type_id",
                        column: x => x.type_id,
                        principalTable: "PromoCodeType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPromoCode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_activated = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    promo_code_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_promo_code", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_promo_code_promo_code_promo_code_id",
                        column: x => x.promo_code_id,
                        principalTable: "PromoCode",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_promo_code_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PromoCodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2fafb979-66e2-4ec5-a5bd-b6b29f270815"), "balance" },
                    { new Guid("713fe6db-768d-4d85-826e-19ccc4428115"), "box" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_promo_code_id",
                table: "PromoCode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promo_code_name",
                table: "PromoCode",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promo_code_type_id",
                table: "PromoCode",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "ix_promo_code_type_id1",
                table: "PromoCodeType",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promo_code_type_name",
                table: "PromoCodeType",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_promo_code_id",
                table: "UserPromoCode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_promo_code_promo_code_id",
                table: "UserPromoCode",
                column: "promo_code_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_promo_code_user_id",
                table: "UserPromoCode",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPromoCode");

            migrationBuilder.DropTable(
                name: "PromoCode");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PromoCodeType");
        }
    }
}
