using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Authentication.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_restriction_user_id",
                table: "UserRestriction");

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("7549d74d-7b55-462f-ad66-c286b3279390"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("c68544ad-f45a-4145-ba19-95b73024f94b"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("c6ff75e9-1ca0-440c-a9f1-1cb033439b63"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("fc375bf3-4e87-4c5a-8143-5d1d47f319d2"));

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0eab92ff-28cf-43aa-94b6-ccf531e60041"), "admin" },
                    { new Guid("82695d93-9d02-459c-b262-39e343ea107e"), "bot" },
                    { new Guid("a217b81f-9078-45f9-98b5-f2a1d82fbc1d"), "owner" },
                    { new Guid("dba56c87-19fc-4c9f-92a1-77e9cb086e10"), "user" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_restriction_user_id",
                table: "UserRestriction",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_restriction_user_id",
                table: "UserRestriction");

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("0eab92ff-28cf-43aa-94b6-ccf531e60041"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("82695d93-9d02-459c-b262-39e343ea107e"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("a217b81f-9078-45f9-98b5-f2a1d82fbc1d"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("dba56c87-19fc-4c9f-92a1-77e9cb086e10"));

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("7549d74d-7b55-462f-ad66-c286b3279390"), "bot" },
                    { new Guid("c68544ad-f45a-4145-ba19-95b73024f94b"), "admin" },
                    { new Guid("c6ff75e9-1ca0-440c-a9f1-1cb033439b63"), "owner" },
                    { new Guid("fc375bf3-4e87-4c5a-8143-5d1d47f319d2"), "user" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_restriction_user_id",
                table: "UserRestriction",
                column: "user_id",
                unique: true);
        }
    }
}
