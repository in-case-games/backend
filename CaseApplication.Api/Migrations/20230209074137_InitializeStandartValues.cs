using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitializeStandartValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "Id", "PromocodeTypeName" },
                values: new object[,]
                {
                    { new Guid("5335710c-a505-4234-b2ee-dc6d91914912"), "balance" },
                    { new Guid("6958df03-49a4-4b3d-aad7-ce927c7cd8fa"), "case" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { new Guid("4bd921c4-4241-4584-bdd4-c26afa095168"), "admin" },
                    { new Guid("5d810a50-6b3d-4c3f-bd35-1db9bd709f8e"), "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "Id",
                keyValue: new Guid("5335710c-a505-4234-b2ee-dc6d91914912"));

            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "Id",
                keyValue: new Guid("6958df03-49a4-4b3d-aad7-ce927c7cd8fa"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: new Guid("4bd921c4-4241-4584-bdd4-c26afa095168"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: new Guid("5d810a50-6b3d-4c3f-bd35-1db9bd709f8e"));
        }
    }
}
