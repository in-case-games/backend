using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class SomeChages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UserBalance",
                table: "UserAdditionalInfo",
                type: "DECIMAL(18,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(6,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserAbleToPay",
                table: "UserAdditionalInfo",
                type: "DECIMAL(18,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(6,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GameItemCost",
                table: "GameItem",
                type: "DECIMAL(18,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(7,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RevenuePrecentage",
                table: "GameCase",
                type: "DECIMAL(18,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(6,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GameCaseCost",
                table: "GameCase",
                type: "DECIMAL(18,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(7,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LossChance",
                table: "CaseInventory",
                type: "DECIMAL(18,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(6,5)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UserBalance",
                table: "UserAdditionalInfo",
                type: "DECIMAL(6,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserAbleToPay",
                table: "UserAdditionalInfo",
                type: "DECIMAL(6,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GameItemCost",
                table: "GameItem",
                type: "DECIMAL(7,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RevenuePrecentage",
                table: "GameCase",
                type: "DECIMAL(6,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GameCaseCost",
                table: "GameCase",
                type: "DECIMAL(7,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LossChance",
                table: "CaseInventory",
                type: "DECIMAL(6,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,5)");
        }
    }
}
