﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Support.DAL.Data;

#nullable disable

namespace Support.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Support.DAL.Entities.AnswerImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AnswerId")
                        .HasColumnType("uuid")
                        .HasColumnName("answer_id");

                    b.HasKey("Id")
                        .HasName("pk_answer_image");

                    b.HasIndex("AnswerId")
                        .HasDatabaseName("ix_answer_image_answer_id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_answer_image_id");

                    b.ToTable("AnswerImage", (string)null);
                });

            modelBuilder.Entity("Support.DAL.Entities.SupportTopic", b =>
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

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_closed");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_support_topic");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_support_topic_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_support_topic_user_id");

                    b.ToTable("SupportTopic", (string)null);
                });

            modelBuilder.Entity("Support.DAL.Entities.SupportTopicAnswer", b =>
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

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<Guid?>("PlaintiffId")
                        .HasColumnType("uuid")
                        .HasColumnName("plaintiff_id");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("uuid")
                        .HasColumnName("topic_id");

                    b.HasKey("Id")
                        .HasName("pk_support_topic_answer");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_support_topic_answer_id");

                    b.HasIndex("PlaintiffId")
                        .HasDatabaseName("ix_support_topic_answer_plaintiff_id");

                    b.HasIndex("TopicId")
                        .HasDatabaseName("ix_support_topic_answer_topic_id");

                    b.ToTable("SupportTopicAnswer", (string)null);
                });

            modelBuilder.Entity("Support.DAL.Entities.User", b =>
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

            modelBuilder.Entity("Support.DAL.Entities.AnswerImage", b =>
                {
                    b.HasOne("Support.DAL.Entities.SupportTopicAnswer", "Answer")
                        .WithMany("Images")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_answer_image_support_topic_answers_answer_id");

                    b.Navigation("Answer");
                });

            modelBuilder.Entity("Support.DAL.Entities.SupportTopic", b =>
                {
                    b.HasOne("Support.DAL.Entities.User", "User")
                        .WithMany("Topics")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_support_topic_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Support.DAL.Entities.SupportTopicAnswer", b =>
                {
                    b.HasOne("Support.DAL.Entities.User", "Plaintiff")
                        .WithMany("Answers")
                        .HasForeignKey("PlaintiffId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_support_topic_answer_users_plaintiff_id");

                    b.HasOne("Support.DAL.Entities.SupportTopic", "Topic")
                        .WithMany("Answers")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_support_topic_answer_support_topics_topic_id");

                    b.Navigation("Plaintiff");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Support.DAL.Entities.SupportTopic", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Support.DAL.Entities.SupportTopicAnswer", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Support.DAL.Entities.User", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Topics");
                });
#pragma warning restore 612, 618
        }
    }
}
