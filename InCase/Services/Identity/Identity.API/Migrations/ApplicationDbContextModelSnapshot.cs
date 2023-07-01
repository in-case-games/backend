﻿// <auto-generated />
using System;
using Identity.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Identity.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Identity.DAL.Entities.RestrictionType", b =>
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
                        .HasName("pk_restriction_type");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_restriction_type_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_restriction_type_name");

                    b.ToTable("RestrictionType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("8d713420-a401-4a69-afde-957b89d9798b"),
                            Name = "mute"
                        },
                        new
                        {
                            Id = new Guid("30c10b8e-06d5-404e-ac48-bdf7259eee0d"),
                            Name = "ban"
                        },
                        new
                        {
                            Id = new Guid("ac0e4746-af26-4209-abe9-6648ce45b59b"),
                            Name = "warn"
                        });
                });

            modelBuilder.Entity("Identity.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("login");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_id");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasDatabaseName("ix_user_login");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Identity.DAL.Entities.UserAdditionalInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deletion_date");

                    b.Property<string>("ImageUri")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_uri");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_additional_info");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_additional_info_id");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_user_additional_info_role_id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_additional_info_user_id");

                    b.ToTable("UserAdditionalInfo", (string)null);
                });

            modelBuilder.Entity("Identity.DAL.Entities.UserRestriction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<string>("Description")
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)")
                        .HasColumnName("description");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("type_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_restriction");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_restriction_id");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_user_restriction_owner_id");

                    b.HasIndex("TypeId")
                        .HasDatabaseName("ix_user_restriction_type_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_restriction_user_id");

                    b.ToTable("UserRestriction", (string)null);
                });

            modelBuilder.Entity("Identity.DAL.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_user_role");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_user_role_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_user_role_name");

                    b.ToTable("UserRole", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("1798c17f-0b31-41b9-8067-7213983b4b93"),
                            Name = "user"
                        },
                        new
                        {
                            Id = new Guid("5a945425-077a-442e-8b54-90694b553384"),
                            Name = "admin"
                        },
                        new
                        {
                            Id = new Guid("62f46100-886f-4919-9513-f1d480f36825"),
                            Name = "owner"
                        },
                        new
                        {
                            Id = new Guid("ec71178e-e08d-45f3-adcb-1e3ab316e6ac"),
                            Name = "bot"
                        });
                });

            modelBuilder.Entity("Identity.DAL.Entities.UserAdditionalInfo", b =>
                {
                    b.HasOne("Identity.DAL.Entities.UserRole", "Role")
                        .WithOne("AdditionalInfo")
                        .HasForeignKey("Identity.DAL.Entities.UserAdditionalInfo", "RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("fk_user_additional_info_roles_role_id");

                    b.HasOne("Identity.DAL.Entities.User", "User")
                        .WithOne("AdditionalInfo")
                        .HasForeignKey("Identity.DAL.Entities.UserAdditionalInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_additional_info_users_user_id");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Identity.DAL.Entities.UserRestriction", b =>
                {
                    b.HasOne("Identity.DAL.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("fk_user_restriction_user_owner_id");

                    b.HasOne("Identity.DAL.Entities.RestrictionType", "Type")
                        .WithOne("Restriction")
                        .HasForeignKey("Identity.DAL.Entities.UserRestriction", "TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_restriction_restriction_type_type_id");

                    b.HasOne("Identity.DAL.Entities.User", "User")
                        .WithMany("Restrictions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_restriction_user_user_id");

                    b.Navigation("Owner");

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Identity.DAL.Entities.RestrictionType", b =>
                {
                    b.Navigation("Restriction");
                });

            modelBuilder.Entity("Identity.DAL.Entities.User", b =>
                {
                    b.Navigation("AdditionalInfo");

                    b.Navigation("Restrictions");
                });

            modelBuilder.Entity("Identity.DAL.Entities.UserRole", b =>
                {
                    b.Navigation("AdditionalInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
