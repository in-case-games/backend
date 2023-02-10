using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseApplication.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "UserRestriction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromocodeExpiryTime",
                table: "Promocode",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "Id", "PromocodeTypeName" },
                values: new object[,]
                {
                    { new Guid("3170302a-64d8-41bb-9f4d-c58a2f918b0c"), "case" },
                    { new Guid("476d6f79-bbf6-4f53-844d-b529e21e4239"), "balance" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { new Guid("c6d009c3-bfc3-4749-ad55-eeb84ffb2bd9"), "user" },
                    { new Guid("d6a61b91-6cef-4c0b-b17b-da2f36e58c57"), "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "Id",
                keyValue: new Guid("3170302a-64d8-41bb-9f4d-c58a2f918b0c"));

            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "Id",
                keyValue: new Guid("476d6f79-bbf6-4f53-844d-b529e21e4239"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: new Guid("c6d009c3-bfc3-4749-ad55-eeb84ffb2bd9"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: new Guid("d6a61b91-6cef-4c0b-b17b-da2f36e58c57"));

            migrationBuilder.DropColumn(
                name: "PromocodeExpiryTime",
                table: "Promocode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "UserRestriction",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
    }
}
