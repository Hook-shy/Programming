﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Programming.Models;

namespace Programming.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20200804052958_MyMigration")]
    partial class MyMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Programming.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Mail")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Nick")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("SessionCode")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("SessionEnd")
                        .HasColumnType("datetime");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<string>("Synopsis")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("UserImgURL")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("varchar(6)")
                        .HasMaxLength(6);

                    b.Property<DateTime>("VerificationEnd")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
