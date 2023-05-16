using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentInvoiceStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_invoice_status", x => x.id);
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
                name: "UserPayments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    rate = table.Column<decimal>(type: "numeric(6,5)", nullable: false),
                    status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_payments_payment_invoice_status_status_id",
                        column: x => x.status_id,
                        principalTable: "PaymentInvoiceStatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_payments_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPromocode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount = table.Column<decimal>(type: "numeric(5,5)", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_promocode", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_promocode_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_payment_invoice_status_id",
                table: "PaymentInvoiceStatus",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payment_invoice_status_name",
                table: "PaymentInvoiceStatus",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_payments_id",
                table: "UserPayments",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_payments_status_id",
                table: "UserPayments",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_payments_user_id",
                table: "UserPayments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_promocode_id",
                table: "UserPromocode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_promocode_user_id",
                table: "UserPromocode",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPayments");

            migrationBuilder.DropTable(
                name: "UserPromocode");

            migrationBuilder.DropTable(
                name: "PaymentInvoiceStatus");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
