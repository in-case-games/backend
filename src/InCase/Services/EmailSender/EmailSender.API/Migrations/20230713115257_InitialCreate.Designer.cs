﻿// <auto-generated />
using System;
using EmailSender.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmailSender.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230713115257_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EmailSender.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("EmailSender.DAL.Entities.UserAdditionalInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsNotifyEmail")
                        .HasColumnType("boolean")
                        .HasColumnName("is_notify_email");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_additional_info");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_additional_info_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_additional_info_user_id");

                    b.ToTable("UserAdditionalInfo", (string)null);
                });

            modelBuilder.Entity("EmailSender.DAL.Entities.UserAdditionalInfo", b =>
                {
                    b.HasOne("EmailSender.DAL.Entities.User", "User")
                        .WithOne("AdditionalInfo")
                        .HasForeignKey("EmailSender.DAL.Entities.UserAdditionalInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_additional_info_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EmailSender.DAL.Entities.User", b =>
                {
                    b.Navigation("AdditionalInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
