﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using xPlanner.Data;

#nullable disable

namespace xPlanner.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240418202256_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("xPlanner.Domain.Entities.PomodoroRound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("PomodoroSessionId")
                        .HasColumnType("integer");

                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.Property<int>("TotalMinutes")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PomodoroSessionId");

                    b.ToTable("PomodoroRounds");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.PomodoroSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PomodoroSessions");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.TimeBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId1")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.ToTable("TimeBlocks");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PomodoroBreakInterval")
                        .HasColumnType("integer");

                    b.Property<int>("PomodoroIntervalsCount")
                        .HasColumnType("integer");

                    b.Property<int>("PomodoroWorkInterval")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UsersSettings");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.UserTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId1")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.PomodoroRound", b =>
                {
                    b.HasOne("xPlanner.Domain.Entities.PomodoroSession", null)
                        .WithMany("Rounds")
                        .HasForeignKey("PomodoroSessionId");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.PomodoroSession", b =>
                {
                    b.HasOne("xPlanner.Domain.Entities.User", null)
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.TimeBlock", b =>
                {
                    b.HasOne("xPlanner.Domain.Entities.User", null)
                        .WithMany("TimeBlocks")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.UserSettings", b =>
                {
                    b.HasOne("xPlanner.Domain.Entities.User", null)
                        .WithOne("Settings")
                        .HasForeignKey("xPlanner.Domain.Entities.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.UserTask", b =>
                {
                    b.HasOne("xPlanner.Domain.Entities.User", null)
                        .WithMany("Tasks")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.PomodoroSession", b =>
                {
                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("xPlanner.Domain.Entities.User", b =>
                {
                    b.Navigation("Sessions");

                    b.Navigation("Settings")
                        .IsRequired();

                    b.Navigation("Tasks");

                    b.Navigation("TimeBlocks");
                });
#pragma warning restore 612, 618
        }
    }
}