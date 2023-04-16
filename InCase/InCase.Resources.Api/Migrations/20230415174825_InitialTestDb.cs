using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InCase.Resources.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialTestDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Game",
                keyColumn: "id",
                keyValue: new Guid("14d45451-5423-4cf3-8d4e-742563024c5e"));

            migrationBuilder.DeleteData(
                table: "Game",
                keyColumn: "id",
                keyValue: new Guid("17f0ea70-f27b-4069-9519-41a6277079f5"));

            migrationBuilder.DeleteData(
                table: "Game",
                keyColumn: "id",
                keyValue: new Guid("e0c84a7f-1f1a-4620-a57a-f3f0100bf634"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("2715a79f-f06f-4539-803a-014b6a0bb824"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("4890cc5d-ac51-4576-8f55-71937600d89c"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("58198e37-c0ba-4429-9ba5-f6906e7cca70"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("72058986-e1b8-453e-9254-42bf80e6ac11"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("caec0fb3-4b94-4b29-bfba-684daa073f36"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("cff44a7c-5a12-402b-a2c1-e184a5e27f42"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("566b5bdc-928a-4164-95cd-bcdfafb8c63f"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("64c70d70-d4f2-4a6b-92db-bfa51729abf1"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("c75d62b4-5f9a-44ed-be67-d090165fa38b"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("d85b2455-18f9-4cfe-b6c7-128deaf02161"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("dde63c79-ee6b-46a0-8b3d-b12f0c99ff5d"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("eabf3771-1cf9-4e95-a4ad-9509ff7fa0c0"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("46ec98bc-14e6-4f38-b602-ea986a80beb1"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("631383b8-f845-478f-80a1-bc4770c78467"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("6fa9cf05-2c58-40d9-871b-7551003627be"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("8ebea4a4-a8a3-4556-a097-7c95e173443e"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("aa6852bd-d674-41fd-9597-1737e1d61e88"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("ac86dc67-480f-4281-ba78-b1eb1892bcc8"));

            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "id",
                keyValue: new Guid("1445d2cc-5c69-46bb-a37d-f3afff20b6de"));

            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "id",
                keyValue: new Guid("7908b93e-fba4-4f8f-9933-c2d3f8d984c4"));

            migrationBuilder.DeleteData(
                table: "RestrictionType",
                keyColumn: "id",
                keyValue: new Guid("1aac136e-de28-4904-82f0-52e6513a846e"));

            migrationBuilder.DeleteData(
                table: "RestrictionType",
                keyColumn: "id",
                keyValue: new Guid("40a5d8c6-0ac4-477c-837b-baff3fa67814"));

            migrationBuilder.DeleteData(
                table: "RestrictionType",
                keyColumn: "id",
                keyValue: new Guid("f7018d83-8abf-4d23-9de3-c64d5d424823"));

            migrationBuilder.DeleteData(
                table: "SiteStatistics",
                keyColumn: "id",
                keyValue: new Guid("4542d888-a2da-4b6a-bc4d-7a3c1cb9aaff"));

            migrationBuilder.DeleteData(
                table: "SiteStatisticsAdmin",
                keyColumn: "id",
                keyValue: new Guid("45970da1-7bc8-4ba8-81f1-3fce060f0a9e"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("2dea2cf5-9609-4151-b8cf-7b550aeaf5c9"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("3a2b33da-f106-4ceb-a8a3-0353e3789055"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("d618c0a2-77b0-48fa-b32b-2834c2ecdcf1"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("fc482a09-881a-43f4-aee6-103cbfe86618"));

            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("5564ae71-fe1a-4240-985c-24fc3f292f29"), "csgo" },
                    { new Guid("6bd2b9e5-d7e9-4c77-8bc5-8447396fe775"), "genshin" },
                    { new Guid("6dd532ff-ffb6-4257-bc30-1325e6c3b3c5"), "dota" }
                });

            migrationBuilder.InsertData(
                table: "GameItemQuality",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("09058ca3-39c3-4fce-9279-85ae10508be1"), "field tested" },
                    { new Guid("0b2841e8-9ae1-4cd6-877b-498a00491de9"), "factory new" },
                    { new Guid("3b5ec317-8a61-4532-b898-afc1525a3c48"), "well worn" },
                    { new Guid("82b5f6ba-a672-40a5-adf7-4e8f5823267a"), "minimal wear" },
                    { new Guid("c729669d-057a-40a3-9d47-ba922827050b"), "battle scarred" },
                    { new Guid("cdd4d1aa-0b20-4af8-8ab6-a5050ee4c648"), "none" }
                });

            migrationBuilder.InsertData(
                table: "GameItemRarity",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1986f9b5-cb07-4a41-b828-5bc336a9d7e9"), "blue" },
                    { new Guid("48cb0213-3b8a-4870-a701-6e1c5eb23b61"), "gold" },
                    { new Guid("500b5fe2-1288-4cf0-8470-f55f770ad949"), "white" },
                    { new Guid("c2c9635d-231e-47f0-b266-ec30fc20a02e"), "pink" },
                    { new Guid("c9e355ac-4dab-442a-a0bc-43daeb8730a2"), "red" },
                    { new Guid("d136e105-1fea-459a-b968-dc9f6ceba884"), "violet" }
                });

            migrationBuilder.InsertData(
                table: "GameItemType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2277266b-f015-43ff-960a-15a5c0a4ece5"), "gloves" },
                    { new Guid("45a2ae70-9c9c-45fa-8f4b-bc4a29b5cc0a"), "none" },
                    { new Guid("480bc8d8-7fd9-4c66-a3e6-e33f8f9e6591"), "knife" },
                    { new Guid("633f8ab2-3ccd-41e1-86bf-2e208be73065"), "weapon" },
                    { new Guid("a7dc09a7-8c68-485a-b8d8-896be3c1c485"), "pistol" },
                    { new Guid("f5ec451c-f4d3-4fa5-8cb9-05154ab32207"), "rifle" }
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2d1482ce-b971-4ee8-a5e9-5585f497c7de"), "case" },
                    { new Guid("afce668a-3a0e-4fc3-8de6-3abb5c777d27"), "balance" }
                });

            migrationBuilder.InsertData(
                table: "RestrictionType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("6a6bf19b-1626-4467-9840-7b3df0f1a978"), "warn" },
                    { new Guid("7bcf501e-5a80-4bc0-8517-9ed8bdf0ccb2"), "mute" },
                    { new Guid("f295122c-46b2-44dc-91c6-5c31fe51d2ff"), "ban" }
                });

            migrationBuilder.InsertData(
                table: "SiteStatistics",
                columns: new[] { "id", "loot_boxes", "reviews", "users", "withdrawn_funds", "withdrawn_items" },
                values: new object[] { new Guid("5cffa140-a37b-44a2-8e50-582f3633ed61"), 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "SiteStatisticsAdmin",
                columns: new[] { "id", "balance_withdrawn", "sent_sites", "total_replenished" },
                values: new object[] { new Guid("65b61b3f-d872-46c9-af63-8f2b4c3d3da1"), 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("019753cc-e47e-49c9-9012-7c09dd0660dd"), "user" },
                    { new Guid("826a3541-1379-48c9-8916-4fa33915e8cf"), "admin" },
                    { new Guid("865ece45-5668-40da-8934-1df83e371cb6"), "owner" },
                    { new Guid("a55a30ec-2bba-4ffb-b019-a0da0dda95ca"), "bot" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Game",
                keyColumn: "id",
                keyValue: new Guid("5564ae71-fe1a-4240-985c-24fc3f292f29"));

            migrationBuilder.DeleteData(
                table: "Game",
                keyColumn: "id",
                keyValue: new Guid("6bd2b9e5-d7e9-4c77-8bc5-8447396fe775"));

            migrationBuilder.DeleteData(
                table: "Game",
                keyColumn: "id",
                keyValue: new Guid("6dd532ff-ffb6-4257-bc30-1325e6c3b3c5"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("09058ca3-39c3-4fce-9279-85ae10508be1"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("0b2841e8-9ae1-4cd6-877b-498a00491de9"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("3b5ec317-8a61-4532-b898-afc1525a3c48"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("82b5f6ba-a672-40a5-adf7-4e8f5823267a"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("c729669d-057a-40a3-9d47-ba922827050b"));

            migrationBuilder.DeleteData(
                table: "GameItemQuality",
                keyColumn: "id",
                keyValue: new Guid("cdd4d1aa-0b20-4af8-8ab6-a5050ee4c648"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("1986f9b5-cb07-4a41-b828-5bc336a9d7e9"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("48cb0213-3b8a-4870-a701-6e1c5eb23b61"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("500b5fe2-1288-4cf0-8470-f55f770ad949"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("c2c9635d-231e-47f0-b266-ec30fc20a02e"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("c9e355ac-4dab-442a-a0bc-43daeb8730a2"));

            migrationBuilder.DeleteData(
                table: "GameItemRarity",
                keyColumn: "id",
                keyValue: new Guid("d136e105-1fea-459a-b968-dc9f6ceba884"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("2277266b-f015-43ff-960a-15a5c0a4ece5"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("45a2ae70-9c9c-45fa-8f4b-bc4a29b5cc0a"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("480bc8d8-7fd9-4c66-a3e6-e33f8f9e6591"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("633f8ab2-3ccd-41e1-86bf-2e208be73065"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("a7dc09a7-8c68-485a-b8d8-896be3c1c485"));

            migrationBuilder.DeleteData(
                table: "GameItemType",
                keyColumn: "id",
                keyValue: new Guid("f5ec451c-f4d3-4fa5-8cb9-05154ab32207"));

            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "id",
                keyValue: new Guid("2d1482ce-b971-4ee8-a5e9-5585f497c7de"));

            migrationBuilder.DeleteData(
                table: "PromocodeType",
                keyColumn: "id",
                keyValue: new Guid("afce668a-3a0e-4fc3-8de6-3abb5c777d27"));

            migrationBuilder.DeleteData(
                table: "RestrictionType",
                keyColumn: "id",
                keyValue: new Guid("6a6bf19b-1626-4467-9840-7b3df0f1a978"));

            migrationBuilder.DeleteData(
                table: "RestrictionType",
                keyColumn: "id",
                keyValue: new Guid("7bcf501e-5a80-4bc0-8517-9ed8bdf0ccb2"));

            migrationBuilder.DeleteData(
                table: "RestrictionType",
                keyColumn: "id",
                keyValue: new Guid("f295122c-46b2-44dc-91c6-5c31fe51d2ff"));

            migrationBuilder.DeleteData(
                table: "SiteStatistics",
                keyColumn: "id",
                keyValue: new Guid("5cffa140-a37b-44a2-8e50-582f3633ed61"));

            migrationBuilder.DeleteData(
                table: "SiteStatisticsAdmin",
                keyColumn: "id",
                keyValue: new Guid("65b61b3f-d872-46c9-af63-8f2b4c3d3da1"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("019753cc-e47e-49c9-9012-7c09dd0660dd"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("826a3541-1379-48c9-8916-4fa33915e8cf"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("865ece45-5668-40da-8934-1df83e371cb6"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: new Guid("a55a30ec-2bba-4ffb-b019-a0da0dda95ca"));

            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("14d45451-5423-4cf3-8d4e-742563024c5e"), "csgo" },
                    { new Guid("17f0ea70-f27b-4069-9519-41a6277079f5"), "dota" },
                    { new Guid("e0c84a7f-1f1a-4620-a57a-f3f0100bf634"), "genshin" }
                });

            migrationBuilder.InsertData(
                table: "GameItemQuality",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2715a79f-f06f-4539-803a-014b6a0bb824"), "none" },
                    { new Guid("4890cc5d-ac51-4576-8f55-71937600d89c"), "battle scarred" },
                    { new Guid("58198e37-c0ba-4429-9ba5-f6906e7cca70"), "factory new" },
                    { new Guid("72058986-e1b8-453e-9254-42bf80e6ac11"), "field tested" },
                    { new Guid("caec0fb3-4b94-4b29-bfba-684daa073f36"), "well worn" },
                    { new Guid("cff44a7c-5a12-402b-a2c1-e184a5e27f42"), "minimal wear" }
                });

            migrationBuilder.InsertData(
                table: "GameItemRarity",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("566b5bdc-928a-4164-95cd-bcdfafb8c63f"), "gold" },
                    { new Guid("64c70d70-d4f2-4a6b-92db-bfa51729abf1"), "red" },
                    { new Guid("c75d62b4-5f9a-44ed-be67-d090165fa38b"), "blue" },
                    { new Guid("d85b2455-18f9-4cfe-b6c7-128deaf02161"), "white" },
                    { new Guid("dde63c79-ee6b-46a0-8b3d-b12f0c99ff5d"), "pink" },
                    { new Guid("eabf3771-1cf9-4e95-a4ad-9509ff7fa0c0"), "violet" }
                });

            migrationBuilder.InsertData(
                table: "GameItemType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("46ec98bc-14e6-4f38-b602-ea986a80beb1"), "weapon" },
                    { new Guid("631383b8-f845-478f-80a1-bc4770c78467"), "rifle" },
                    { new Guid("6fa9cf05-2c58-40d9-871b-7551003627be"), "gloves" },
                    { new Guid("8ebea4a4-a8a3-4556-a097-7c95e173443e"), "knife" },
                    { new Guid("aa6852bd-d674-41fd-9597-1737e1d61e88"), "pistol" },
                    { new Guid("ac86dc67-480f-4281-ba78-b1eb1892bcc8"), "none" }
                });

            migrationBuilder.InsertData(
                table: "PromocodeType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1445d2cc-5c69-46bb-a37d-f3afff20b6de"), "balance" },
                    { new Guid("7908b93e-fba4-4f8f-9933-c2d3f8d984c4"), "case" }
                });

            migrationBuilder.InsertData(
                table: "RestrictionType",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1aac136e-de28-4904-82f0-52e6513a846e"), "ban" },
                    { new Guid("40a5d8c6-0ac4-477c-837b-baff3fa67814"), "mute" },
                    { new Guid("f7018d83-8abf-4d23-9de3-c64d5d424823"), "warn" }
                });

            migrationBuilder.InsertData(
                table: "SiteStatistics",
                columns: new[] { "id", "loot_boxes", "reviews", "users", "withdrawn_funds", "withdrawn_items" },
                values: new object[] { new Guid("4542d888-a2da-4b6a-bc4d-7a3c1cb9aaff"), 0, 0, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "SiteStatisticsAdmin",
                columns: new[] { "id", "balance_withdrawn", "sent_sites", "total_replenished" },
                values: new object[] { new Guid("45970da1-7bc8-4ba8-81f1-3fce060f0a9e"), 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2dea2cf5-9609-4151-b8cf-7b550aeaf5c9"), "owner" },
                    { new Guid("3a2b33da-f106-4ceb-a8a3-0353e3789055"), "admin" },
                    { new Guid("d618c0a2-77b0-48fa-b32b-2834c2ecdcf1"), "user" },
                    { new Guid("fc482a09-881a-43f4-aee6-103cbfe86618"), "bot" }
                });
        }
    }
}
