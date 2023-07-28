﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Resources.DAL.Data;

#nullable disable

namespace Resources.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230527063722_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Resources.DAL.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_game");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_game_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_game_name");

                    b.ToTable("Game", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("0f19ad4d-b81f-4b54-a7fe-280ad6f718ae"),
                            Name = "csgo"
                        },
                        new
                        {
                            Id = new Guid("815821f5-4298-4f19-ac08-de104dbbfbc3"),
                            Name = "dota2"
                        });
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Cost")
                        .HasColumnType("DECIMAL(18,5)")
                        .HasColumnName("cost");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid")
                        .HasColumnName("game_id");

                    b.Property<string>("HashName")
                        .HasColumnType("text")
                        .HasColumnName("hash_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<Guid>("QualityId")
                        .HasColumnType("uuid")
                        .HasColumnName("quality_id");

                    b.Property<Guid>("RarityId")
                        .HasColumnType("uuid")
                        .HasColumnName("rarity_id");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("type_id");

                    b.HasKey("Id")
                        .HasName("pk_game_item");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_game_item_game_id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_game_item_id");

                    b.HasIndex("QualityId")
                        .HasDatabaseName("ix_game_item_quality_id");

                    b.HasIndex("RarityId")
                        .HasDatabaseName("ix_game_item_rarity_id");

                    b.HasIndex("TypeId")
                        .HasDatabaseName("ix_game_item_type_id");

                    b.ToTable("GameItem", (string)null);
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItemQuality", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_game_item_quality");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_game_item_quality_id1");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_game_item_quality_name");

                    b.ToTable("GameItemQuality", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("89ef7b69-b1aa-42f6-ae88-a693ba204d79"),
                            Name = "none"
                        },
                        new
                        {
                            Id = new Guid("9cbf7a68-45aa-43ee-9a22-5062c1fa7160"),
                            Name = "battle scarred"
                        },
                        new
                        {
                            Id = new Guid("3b907aab-909d-4474-8a25-f416dbcc46fc"),
                            Name = "well worn"
                        },
                        new
                        {
                            Id = new Guid("88fef02d-1c27-4a94-a5e8-1770aa5c008d"),
                            Name = "field tested"
                        },
                        new
                        {
                            Id = new Guid("e6c4db8b-5c39-4278-946a-de774224dd3d"),
                            Name = "minimal wear"
                        },
                        new
                        {
                            Id = new Guid("54e6de3f-1011-4158-8c97-7ab75611ff7b"),
                            Name = "factory new"
                        });
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItemRarity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_game_item_rarity");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_game_item_rarity_id1");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_game_item_rarity_name");

                    b.ToTable("GameItemRarity", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("e7a4d888-5d76-4570-b8ae-557acdca77eb"),
                            Name = "white"
                        },
                        new
                        {
                            Id = new Guid("cd697f6d-6296-46d8-a7c7-a8ea9100097a"),
                            Name = "blue"
                        },
                        new
                        {
                            Id = new Guid("41655e50-8e9e-4f00-b9e7-07a9a28d101d"),
                            Name = "violet"
                        },
                        new
                        {
                            Id = new Guid("e468368b-c946-4af1-8c2c-25232e646e4f"),
                            Name = "pink"
                        },
                        new
                        {
                            Id = new Guid("5e692aac-b7be-45dc-8995-9c6e350aa24a"),
                            Name = "red"
                        },
                        new
                        {
                            Id = new Guid("73bdd863-14f6-4ee6-9b3a-a22a60f4544e"),
                            Name = "gold"
                        });
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItemType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_game_item_type");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_game_item_type_id1");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_game_item_type_name");

                    b.ToTable("GameItemType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("aa621834-6051-4c76-b6df-46bb40e47e64"),
                            Name = "none"
                        },
                        new
                        {
                            Id = new Guid("c90556fe-e827-4a14-9421-e6bdd1cab749"),
                            Name = "pistol"
                        },
                        new
                        {
                            Id = new Guid("afbaadbb-6683-436d-985d-fec33f65b6f0"),
                            Name = "weapon"
                        },
                        new
                        {
                            Id = new Guid("98739567-205c-4198-a08a-d860ffff208b"),
                            Name = "rifle"
                        },
                        new
                        {
                            Id = new Guid("2cd08728-ce4c-4d5e-832b-a29b582e68ab"),
                            Name = "knife"
                        },
                        new
                        {
                            Id = new Guid("2c8223d1-9ad3-43e4-b5f9-f91b5088e524"),
                            Name = "gloves"
                        },
                        new
                        {
                            Id = new Guid("d6788b0d-b927-4c2c-b460-167d4d22317e"),
                            Name = "other"
                        });
                });

            modelBuilder.Entity("Resources.DAL.Entities.GroupLootBox", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_group_loot_box");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_group_loot_box_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_group_loot_box_name");

                    b.ToTable("GroupLootBox", (string)null);
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBox", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Cost")
                        .HasColumnType("DECIMAL(18,5)")
                        .HasColumnName("cost");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid")
                        .HasColumnName("game_id");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_locked");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_loot_box");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_loot_box_game_id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_loot_box_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_loot_box_name");

                    b.ToTable("LootBox", (string)null);
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBoxBanner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BoxId")
                        .HasColumnType("uuid")
                        .HasColumnName("box_id");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date");

                    b.HasKey("Id")
                        .HasName("pk_loot_box_banner");

                    b.HasIndex("BoxId")
                        .IsUnique()
                        .HasDatabaseName("ix_loot_box_banner_box_id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_loot_box_banner_id");

                    b.ToTable("LootBoxBanner", (string)null);
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBoxGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BoxId")
                        .HasColumnType("uuid")
                        .HasColumnName("box_id");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid")
                        .HasColumnName("game_id");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.HasKey("Id")
                        .HasName("pk_loot_box_group");

                    b.HasIndex("BoxId")
                        .HasDatabaseName("ix_loot_box_group_box_id");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_loot_box_group_game_id");

                    b.HasIndex("GroupId")
                        .HasDatabaseName("ix_loot_box_group_group_id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_loot_box_group_id");

                    b.ToTable("LootBoxGroup", (string)null);
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBoxInventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BoxId")
                        .HasColumnType("uuid")
                        .HasColumnName("box_id");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.HasKey("Id")
                        .HasName("pk_loot_box_inventory");

                    b.HasIndex("BoxId")
                        .HasDatabaseName("ix_loot_box_inventory_box_id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_loot_box_inventory_id");

                    b.HasIndex("ItemId")
                        .HasDatabaseName("ix_loot_box_inventory_item_id");

                    b.ToTable("LootBoxInventory", (string)null);
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItem", b =>
                {
                    b.HasOne("Resources.DAL.Entities.Game", "Game")
                        .WithMany("Items")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_game_item_game_game_id");

                    b.HasOne("Resources.DAL.Entities.GameItemQuality", "Quality")
                        .WithOne("Item")
                        .HasForeignKey("Resources.DAL.Entities.GameItem", "QualityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_game_item_item_qualities_quality_id");

                    b.HasOne("Resources.DAL.Entities.GameItemRarity", "Rarity")
                        .WithOne("Item")
                        .HasForeignKey("Resources.DAL.Entities.GameItem", "RarityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_game_item_item_rarities_rarity_id");

                    b.HasOne("Resources.DAL.Entities.GameItemType", "Type")
                        .WithOne("Item")
                        .HasForeignKey("Resources.DAL.Entities.GameItem", "TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_game_item_item_types_type_id");

                    b.Navigation("Game");

                    b.Navigation("Quality");

                    b.Navigation("Rarity");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBox", b =>
                {
                    b.HasOne("Resources.DAL.Entities.Game", "Game")
                        .WithMany("Boxes")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_loot_box_game_game_id");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBoxBanner", b =>
                {
                    b.HasOne("Resources.DAL.Entities.LootBox", "Box")
                        .WithOne("Banner")
                        .HasForeignKey("Resources.DAL.Entities.LootBoxBanner", "BoxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_loot_box_banner_loot_boxes_box_id");

                    b.Navigation("Box");
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBoxGroup", b =>
                {
                    b.HasOne("Resources.DAL.Entities.LootBox", "Box")
                        .WithMany("Groups")
                        .HasForeignKey("BoxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_loot_box_group_loot_box_box_id");

                    b.HasOne("Resources.DAL.Entities.Game", "Game")
                        .WithMany("Groups")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_loot_box_group_game_game_id");

                    b.HasOne("Resources.DAL.Entities.GroupLootBox", "Group")
                        .WithOne("Group")
                        .HasForeignKey("Resources.DAL.Entities.LootBoxGroup", "GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_loot_box_group_group_loot_box_group_id");

                    b.Navigation("Box");

                    b.Navigation("Game");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBoxInventory", b =>
                {
                    b.HasOne("Resources.DAL.Entities.LootBox", "Box")
                        .WithMany("Inventories")
                        .HasForeignKey("BoxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_loot_box_inventory_loot_box_box_id");

                    b.HasOne("Resources.DAL.Entities.GameItem", "Item")
                        .WithMany("Inventories")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_loot_box_inventory_game_item_item_id");

                    b.Navigation("Box");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Resources.DAL.Entities.Game", b =>
                {
                    b.Navigation("Boxes");

                    b.Navigation("Groups");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItem", b =>
                {
                    b.Navigation("Inventories");
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItemQuality", b =>
                {
                    b.Navigation("Item");
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItemRarity", b =>
                {
                    b.Navigation("Item");
                });

            modelBuilder.Entity("Resources.DAL.Entities.GameItemType", b =>
                {
                    b.Navigation("Item");
                });

            modelBuilder.Entity("Resources.DAL.Entities.GroupLootBox", b =>
                {
                    b.Navigation("Group");
                });

            modelBuilder.Entity("Resources.DAL.Entities.LootBox", b =>
                {
                    b.Navigation("Banner");

                    b.Navigation("Groups");

                    b.Navigation("Inventories");
                });
#pragma warning restore 612, 618
        }
    }
}