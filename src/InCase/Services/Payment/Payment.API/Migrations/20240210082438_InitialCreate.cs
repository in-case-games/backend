using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Payment.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_status", x => x.id);
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
                name: "UserPayment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    currency = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_payment", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_payment_payment_status_status_id",
                        column: x => x.status_id,
                        principalTable: "PaymentStatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_payment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPromoCode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount = table.Column<decimal>(type: "numeric(5,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_promo_code", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_promo_code_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("3f5584fe-3420-4ecd-959a-f8380473ef7e"), "waiting" },
                    { new Guid("75de0151-2d2e-4161-b016-8f70656d10af"), "succeeded" },
                    { new Guid("99d02ba4-9ed3-4295-97ad-8dfc1ca34e97"), "canceled" },
                    { new Guid("af01255d-15b7-4d34-ae01-2fcb3769dfd2"), "pending" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_payment_status_id",
                table: "PaymentStatus",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payment_status_name",
                table: "PaymentStatus",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_payment_id",
                table: "UserPayment",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_payment_status_id",
                table: "UserPayment",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_payment_user_id",
                table: "UserPayment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_promo_code_id",
                table: "UserPromoCode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_promo_code_user_id",
                table: "UserPromoCode",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPayment");

            migrationBuilder.DropTable(
                name: "UserPromoCode");

            migrationBuilder.DropTable(
                name: "PaymentStatus");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
