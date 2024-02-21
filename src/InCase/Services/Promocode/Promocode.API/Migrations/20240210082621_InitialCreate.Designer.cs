﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Promocode.DAL.Data;

#nullable disable

namespace Promocode.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240210082621_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Promocode.DAL.Entities.PromoCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Discount")
                        .HasColumnType("DECIMAL(5,5)")
                        .HasColumnName("discount");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<int>("NumberActivations")
                        .HasColumnType("integer")
                        .HasColumnName("number_activations");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("type_id");

                    b.HasKey("Id")
                        .HasName("pk_promo_code");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_promo_code_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_promo_code_name");

                    b.HasIndex("TypeId")
                        .HasDatabaseName("ix_promo_code_type_id");

                    b.ToTable("PromoCode", (string)null);
                });

            modelBuilder.Entity("Promocode.DAL.Entities.PromoCodeType", b =>
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
                        .HasName("pk_promo_code_type");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_promo_code_type_id1");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_promo_code_type_name");

                    b.ToTable("PromoCodeType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("2fafb979-66e2-4ec5-a5bd-b6b29f270815"),
                            Name = "balance"
                        },
                        new
                        {
                            Id = new Guid("713fe6db-768d-4d85-826e-19ccc4428115"),
                            Name = "box"
                        });
                });

            modelBuilder.Entity("Promocode.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Promocode.DAL.Entities.UserPromoCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_activated");

                    b.Property<Guid>("PromoCodeId")
                        .HasColumnType("uuid")
                        .HasColumnName("promo_code_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_promo_code");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_promo_code_id");

                    b.HasIndex("PromoCodeId")
                        .HasDatabaseName("ix_user_promo_code_promo_code_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_promo_code_user_id");

                    b.ToTable("UserPromoCode", (string)null);
                });

            modelBuilder.Entity("Promocode.DAL.Entities.PromoCode", b =>
                {
                    b.HasOne("Promocode.DAL.Entities.PromoCodeType", "Type")
                        .WithOne("PromoCode")
                        .HasForeignKey("Promocode.DAL.Entities.PromoCode", "TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_promo_code_promo_codes_types_type_id");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Promocode.DAL.Entities.UserPromoCode", b =>
                {
                    b.HasOne("Promocode.DAL.Entities.PromoCode", "PromoCode")
                        .WithMany("HistoriesPromoCodes")
                        .HasForeignKey("PromoCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_promo_code_promo_code_promo_code_id");

                    b.HasOne("Promocode.DAL.Entities.User", "User")
                        .WithMany("HistoriesPromoCodes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_promo_code_user_user_id");

                    b.Navigation("PromoCode");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Promocode.DAL.Entities.PromoCode", b =>
                {
                    b.Navigation("HistoriesPromoCodes");
                });

            modelBuilder.Entity("Promocode.DAL.Entities.PromoCodeType", b =>
                {
                    b.Navigation("PromoCode");
                });

            modelBuilder.Entity("Promocode.DAL.Entities.User", b =>
                {
                    b.Navigation("HistoriesPromoCodes");
                });
#pragma warning restore 612, 618
        }
    }
}