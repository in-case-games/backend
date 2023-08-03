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
                name: "PromocodeType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promocode_type", x => x.id);
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
                name: "PromocodeEntity",
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
                    table.PrimaryKey("pk_promocode_entity", x => x.id);
                    table.ForeignKey(
                        name: "fk_promocode_entity_promocodes_types_type_id",
                        column: x => x.type_id,
                        principalTable: "PromocodeType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPromocode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_activated = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    promocode_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_promocode", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_promocode_promocode_entity_promocode_id",
                        column: x => x.promocode_id,
                        principalTable: "PromocodeEntity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_promocode_user_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0d91a9a1-2c32-4393-80f8-c1f296060417"), "box" },
                    { new Guid("d4dddc0d-869c-4793-bb8d-024206baac53"), "balance" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_promocode_entity_id",
                table: "PromocodeEntity",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promocode_entity_name",
                table: "PromocodeEntity",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promocode_entity_type_id",
                table: "PromocodeEntity",
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
                name: "ix_user_id",
                table: "User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_promocode_id",
                table: "UserPromocode",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_promocode_promocode_id",
                table: "UserPromocode",
                column: "promocode_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_promocode_user_id",
                table: "UserPromocode",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPromocode");

            migrationBuilder.DropTable(
                name: "PromocodeEntity");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PromocodeType");
        }
    }
}
