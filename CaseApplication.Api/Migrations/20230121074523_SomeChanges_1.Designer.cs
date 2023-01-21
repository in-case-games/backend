﻿// <auto-generated />
using System;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CaseApplication.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230121074523_SomeChanges_1")]
    partial class SomeChanges1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.CaseInventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameCaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("LossChance")
                        .IsRequired()
                        .HasColumnType("DECIMAL(6, 5)");

                    b.Property<int>("NumberItemsCase")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameCaseId")
                        .IsUnique();

                    b.HasIndex("GameItemId")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("CaseInventory");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.GameCase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("GameCaseCost")
                        .HasColumnType("DECIMAL(7, 5)");

                    b.Property<string>("GameCaseImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameCaseName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("RevenuePrecentage")
                        .HasColumnType("DECIMAL(6, 5)");

                    b.HasKey("Id");

                    b.HasIndex("GameCaseName")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("GameCase");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.GameItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("GameItemCost")
                        .HasColumnType("DECIMAL(7, 5)");

                    b.Property<string>("GameItemImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameItemName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("GameItemRarity")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("GameItemName")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("GameItem");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserEmail")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserAdditionalInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("UserAbleToPay")
                        .HasColumnType("DECIMAL(6, 5)");

                    b.Property<int>("UserAge")
                        .HasColumnType("int");

                    b.Property<decimal>("UserBalance")
                        .HasColumnType("DECIMAL(6, 5)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("UserRoleId")
                        .IsUnique();

                    b.ToTable("UserAdditionalInfo");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserInventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameItemId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserInventory");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserRestriction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RestrictionName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserRestriction");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.CaseInventory", b =>
                {
                    b.HasOne("CaseApplication.DomainLayer.Entities.GameCase", "GameCase")
                        .WithMany("СaseInventories")
                        .HasForeignKey("GameCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.DomainLayer.Entities.GameItem", "GameItem")
                        .WithMany("CaseInventories")
                        .HasForeignKey("GameItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameCase");

                    b.Navigation("GameItem");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserAdditionalInfo", b =>
                {
                    b.HasOne("CaseApplication.DomainLayer.Entities.User", "User")
                        .WithOne("UserAdditionalInfo")
                        .HasForeignKey("CaseApplication.DomainLayer.Entities.UserAdditionalInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.DomainLayer.Entities.UserRole", "UserRole")
                        .WithOne("UserAdditionalInfo")
                        .HasForeignKey("CaseApplication.DomainLayer.Entities.UserAdditionalInfo", "UserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserInventory", b =>
                {
                    b.HasOne("CaseApplication.DomainLayer.Entities.GameItem", "GameItem")
                        .WithMany("UserInventories")
                        .HasForeignKey("GameItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.DomainLayer.Entities.User", "User")
                        .WithMany("UserInventories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserRestriction", b =>
                {
                    b.HasOne("CaseApplication.DomainLayer.Entities.User", "User")
                        .WithMany("UserRestrictions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.GameCase", b =>
                {
                    b.Navigation("СaseInventories");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.GameItem", b =>
                {
                    b.Navigation("CaseInventories");

                    b.Navigation("UserInventories");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.User", b =>
                {
                    b.Navigation("UserAdditionalInfo");

                    b.Navigation("UserInventories");

                    b.Navigation("UserRestrictions");
                });

            modelBuilder.Entity("CaseApplication.DomainLayer.Entities.UserRole", b =>
                {
                    b.Navigation("UserAdditionalInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
