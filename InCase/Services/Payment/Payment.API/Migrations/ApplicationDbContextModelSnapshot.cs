﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Payment.DAL.Data;

#nullable disable

namespace Payment.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Payment.DAL.Entities.User", b =>
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

            modelBuilder.Entity("Payment.DAL.Entities.UserPayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("DECIMAL(18,5)")
                        .HasColumnName("amount");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<string>("InvoiceId")
                        .HasColumnType("text")
                        .HasColumnName("invoice_id");

                    b.Property<decimal>("Rate")
                        .HasColumnType("DECIMAL(6,5)")
                        .HasColumnName("rate");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_payment");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_payment_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_payment_user_id");

                    b.ToTable("UserPayment", (string)null);
                });

            modelBuilder.Entity("Payment.DAL.Entities.UserPromocode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Discount")
                        .HasColumnType("DECIMAL(5,5)")
                        .HasColumnName("discount");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_promocode");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_promocode_id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_promocode_user_id");

                    b.ToTable("UserPromocode", (string)null);
                });

            modelBuilder.Entity("Payment.DAL.Entities.UserPayment", b =>
                {
                    b.HasOne("Payment.DAL.Entities.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_payment_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Payment.DAL.Entities.UserPromocode", b =>
                {
                    b.HasOne("Payment.DAL.Entities.User", "User")
                        .WithOne("Promocode")
                        .HasForeignKey("Payment.DAL.Entities.UserPromocode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_promocode_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Payment.DAL.Entities.User", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Promocode");
                });
#pragma warning restore 612, 618
        }
    }
}
