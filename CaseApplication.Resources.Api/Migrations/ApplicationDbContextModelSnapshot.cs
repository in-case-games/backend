﻿// <auto-generated />
using System;
using CaseApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CaseApplication.Resources.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CaseApplication.Domain.Entities.CaseInventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameCaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LossChance")
                        .HasColumnType("int");

                    b.Property<int>("NumberItemsCase")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameCaseId");

                    b.HasIndex("GameItemId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("CaseInventory");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.GameCase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("GameCaseBalance")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.Property<decimal>("GameCaseCost")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.Property<string>("GameCaseImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameCaseName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("GroupCasesName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("RevenuePrecentage")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.HasKey("Id");

                    b.HasIndex("GameCaseName")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("GameCase");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.GameItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("GameItemCost")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.Property<string>("GameItemImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameItemName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GameItemRarity")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("GameItemType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameItemName")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("GameItem");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NewsContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("NewsDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewsImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewsName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("News");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.Promocode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PromocodeDiscount")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.Property<DateTime?>("PromocodeExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PromocodeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("PromocodeTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PromocodeUsesCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PromocodeName")
                        .IsUnique()
                        .HasFilter("[PromocodeName] IS NOT NULL");

                    b.HasIndex("PromocodeTypeId");

                    b.ToTable("Promocode");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.PromocodeType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PromocodeTypeName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PromocodeTypeName")
                        .IsUnique()
                        .HasFilter("[PromocodeTypeName] IS NOT NULL");

                    b.ToTable("PromocodeType");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ef0f0877-aa8d-45f9-aa8f-7f5204045de6"),
                            PromocodeTypeName = "balance"
                        },
                        new
                        {
                            Id = new Guid("6fef7fdd-0d75-40d0-a65e-91940e433717"),
                            PromocodeTypeName = "case"
                        });
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.PromocodesUsedByUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PromocodeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PromocodeId");

                    b.HasIndex("UserId");

                    b.ToTable("PromocodeUsedByUsers");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.SiteStatistics", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CasesOpened")
                        .HasColumnType("int");

                    b.Property<int>("ItemWithdrawn")
                        .HasColumnType("int");

                    b.Property<int>("ReviewsWriten")
                        .HasColumnType("int");

                    b.Property<decimal>("SiteBalance")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.Property<int>("UsersCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("SiteStatistics");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserLogin")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PasswordSalt")
                        .IsUnique();

                    b.HasIndex("UserEmail")
                        .IsUnique();

                    b.HasIndex("UserLogin")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserAdditionalInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsConfirmedAccount")
                        .HasColumnType("bit");

                    b.Property<decimal>("UserAbleToPay")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.Property<decimal>("UserBalance")
                        .HasColumnType("DECIMAL(18, 5)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("UserRoleId");

                    b.ToTable("UserAdditionalInfo");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserHistoryOpeningCases", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CaseOpenAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GameCaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameCaseId");

                    b.HasIndex("GameItemId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserHistoryOpeningCases");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserInventory", b =>
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

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserRestriction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpiryDate")
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

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserRole", b =>
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("17b55ddc-5ff0-452b-b92b-621f6b6e5a4c"),
                            RoleName = "user"
                        },
                        new
                        {
                            Id = new Guid("1b2353ad-d2ca-413c-8fad-ed94ce2ab328"),
                            RoleName = "admin"
                        });
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenCreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserIpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPlatfrom")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.CaseInventory", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.GameCase", "GameCase")
                        .WithMany("СaseInventories")
                        .HasForeignKey("GameCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.Domain.Entities.GameItem", "GameItem")
                        .WithMany("CaseInventories")
                        .HasForeignKey("GameItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameCase");

                    b.Navigation("GameItem");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.Promocode", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.PromocodeType", "PromocodeType")
                        .WithOne("Promocode")
                        .HasForeignKey("CaseApplication.Domain.Entities.Promocode", "PromocodeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PromocodeType");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.PromocodesUsedByUser", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.Promocode", "Promocode")
                        .WithMany("PromocodesUsedByUsers")
                        .HasForeignKey("PromocodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.Domain.Entities.User", "User")
                        .WithMany("PromocodesUsedByUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Promocode");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserAdditionalInfo", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.User", "User")
                        .WithOne("UserAdditionalInfo")
                        .HasForeignKey("CaseApplication.Domain.Entities.UserAdditionalInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.Domain.Entities.UserRole", "UserRole")
                        .WithOne("UserAdditionalInfo")
                        .HasForeignKey("CaseApplication.Domain.Entities.UserAdditionalInfo", "UserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserHistoryOpeningCases", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.GameCase", "GameCase")
                        .WithMany("UserHistoryOpeningCases")
                        .HasForeignKey("GameCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.Domain.Entities.GameItem", "GameItem")
                        .WithMany("UserHistoryOpeningCases")
                        .HasForeignKey("GameItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.Domain.Entities.User", "User")
                        .WithMany("UserHistoryOpeningCases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameCase");

                    b.Navigation("GameItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserInventory", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.GameItem", "GameItem")
                        .WithMany("UserInventories")
                        .HasForeignKey("GameItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseApplication.Domain.Entities.User", "User")
                        .WithMany("UserInventories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserRestriction", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.User", "User")
                        .WithMany("UserRestrictions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserToken", b =>
                {
                    b.HasOne("CaseApplication.Domain.Entities.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.GameCase", b =>
                {
                    b.Navigation("UserHistoryOpeningCases");

                    b.Navigation("СaseInventories");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.GameItem", b =>
                {
                    b.Navigation("CaseInventories");

                    b.Navigation("UserHistoryOpeningCases");

                    b.Navigation("UserInventories");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.Promocode", b =>
                {
                    b.Navigation("PromocodesUsedByUsers");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.PromocodeType", b =>
                {
                    b.Navigation("Promocode");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.User", b =>
                {
                    b.Navigation("PromocodesUsedByUsers");

                    b.Navigation("UserAdditionalInfo");

                    b.Navigation("UserHistoryOpeningCases");

                    b.Navigation("UserInventories");

                    b.Navigation("UserRestrictions");

                    b.Navigation("UserTokens");
                });

            modelBuilder.Entity("CaseApplication.Domain.Entities.UserRole", b =>
                {
                    b.Navigation("UserAdditionalInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
