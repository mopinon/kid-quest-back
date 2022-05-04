﻿// <auto-generated />
using System;
using KidQquest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace kid_quest_back.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("KidQquest.Models.AnswerVariantModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRight")
                        .HasColumnType("boolean");

                    b.Property<int?>("PreviewId")
                        .HasColumnType("integer");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PreviewId");

                    b.HasIndex("QuestionId");

                    b.ToTable("AnswerVariants");
                });

            modelBuilder.Entity("KidQquest.Models.AttemptModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CorrectAnswersCount")
                        .HasColumnType("integer");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuestId");

                    b.HasIndex("UserId");

                    b.ToTable("Attempts");
                });

            modelBuilder.Entity("KidQquest.Models.AwardTypeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PreviewId")
                        .HasColumnType("integer");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PreviewId");

                    b.HasIndex("QuestId")
                        .IsUnique();

                    b.ToTable("AwardTypes");
                });

            modelBuilder.Entity("KidQquest.Models.FactModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("PreviewId")
                        .HasColumnType("integer");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PreviewId");

                    b.HasIndex("QuestionId")
                        .IsUnique();

                    b.ToTable("Facts");
                });

            modelBuilder.Entity("KidQquest.Models.PreviewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("DocType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Previews");
                });

            modelBuilder.Entity("KidQquest.Models.QuestCategoryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("QuestCategories");
                });

            modelBuilder.Entity("KidQquest.Models.QuestModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PreviewId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PreviewId");

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("KidQquest.Models.QuestionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int?>("PreviewId")
                        .HasColumnType("integer");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PreviewId");

                    b.HasIndex("QuestId");

                    b.HasIndex("TypeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("KidQquest.Models.QuestionTypeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("QuestionTypes");
                });

            modelBuilder.Entity("KidQquest.Models.ResetPasswordModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ResetPasswords");
                });

            modelBuilder.Entity("KidQquest.Models.UserAwardModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AwardTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AwardTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAwards");
                });

            modelBuilder.Entity("KidQquest.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailConfirmationCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserStatusString")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KidQquest.Models.AnswerVariantModel", b =>
                {
                    b.HasOne("KidQquest.Models.PreviewModel", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId");

                    b.HasOne("KidQquest.Models.QuestionModel", "Question")
                        .WithMany("AnswerVariants")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preview");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("KidQquest.Models.AttemptModel", b =>
                {
                    b.HasOne("KidQquest.Models.QuestModel", "Quest")
                        .WithMany()
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KidQquest.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KidQquest.Models.AwardTypeModel", b =>
                {
                    b.HasOne("KidQquest.Models.PreviewModel", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KidQquest.Models.QuestModel", "Quest")
                        .WithOne("AwardType")
                        .HasForeignKey("KidQquest.Models.AwardTypeModel", "QuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preview");

                    b.Navigation("Quest");
                });

            modelBuilder.Entity("KidQquest.Models.FactModel", b =>
                {
                    b.HasOne("KidQquest.Models.PreviewModel", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId");

                    b.HasOne("KidQquest.Models.QuestionModel", "Question")
                        .WithOne("Fact")
                        .HasForeignKey("KidQquest.Models.FactModel", "QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preview");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("KidQquest.Models.QuestModel", b =>
                {
                    b.HasOne("KidQquest.Models.QuestCategoryModel", "Category")
                        .WithMany("Quests")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KidQquest.Models.PreviewModel", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Preview");
                });

            modelBuilder.Entity("KidQquest.Models.QuestionModel", b =>
                {
                    b.HasOne("KidQquest.Models.PreviewModel", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId");

                    b.HasOne("KidQquest.Models.QuestModel", "Quest")
                        .WithMany("Questions")
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KidQquest.Models.QuestionTypeModel", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preview");

                    b.Navigation("Quest");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("KidQquest.Models.ResetPasswordModel", b =>
                {
                    b.HasOne("KidQquest.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KidQquest.Models.UserAwardModel", b =>
                {
                    b.HasOne("KidQquest.Models.AwardTypeModel", "AwardType")
                        .WithMany()
                        .HasForeignKey("AwardTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KidQquest.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AwardType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KidQquest.Models.QuestCategoryModel", b =>
                {
                    b.Navigation("Quests");
                });

            modelBuilder.Entity("KidQquest.Models.QuestModel", b =>
                {
                    b.Navigation("AwardType")
                        .IsRequired();

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("KidQquest.Models.QuestionModel", b =>
                {
                    b.Navigation("AnswerVariants");

                    b.Navigation("Fact");
                });
#pragma warning restore 612, 618
        }
    }
}
