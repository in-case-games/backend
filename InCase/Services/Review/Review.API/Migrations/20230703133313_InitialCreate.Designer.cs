﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Review.DAL.Data;

#nullable disable

namespace Review.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230703133313_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Review.DAL.Entities.ReviewImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ReviewId")
                        .HasColumnType("uuid")
                        .HasColumnName("review_id");

                    b.HasKey("Id")
                        .HasName("pk_review_image");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_review_image_id");

                    b.HasIndex("ReviewId")
                        .HasDatabaseName("ix_review_image_review_id");

                    b.ToTable("ReviewImage", (string)null);
                });

            modelBuilder.Entity("Review.DAL.Entities.User", b =>
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

            modelBuilder.Entity("Review.DAL.Entities.UserReview", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("boolean")
                        .HasColumnName("is_approved");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_review");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_review_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_review_user_id");

                    b.ToTable("UserReview", (string)null);
                });

            modelBuilder.Entity("Review.DAL.Entities.ReviewImage", b =>
                {
                    b.HasOne("Review.DAL.Entities.UserReview", "Review")
                        .WithMany("Images")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_review_image_reviews_review_id");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("Review.DAL.Entities.UserReview", b =>
                {
                    b.HasOne("Review.DAL.Entities.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_review_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Review.DAL.Entities.User", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Review.DAL.Entities.UserReview", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
