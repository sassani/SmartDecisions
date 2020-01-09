﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OAuthService.DataBase.Persistence;

namespace OAuthService.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20200108171901_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RestApi.Core.Domain.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClientPublicId")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasColumnType("varchar(75) CHARACTER SET utf8mb4")
                        .HasMaxLength(75);

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Client");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClientPublicId = "cuv12t7",
                            ClientSecret = "$5$10$mFiFWP2TGzuYtOEcH6ymaB$L48XHvOZDumRmt2euRv9b2pRJ93pOOMLsgIPAmAyBsg=",
                            Name = "localhost",
                            Type = 1
                        });
                });

            modelBuilder.Entity("RestApi.Core.Domain.Credential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25)
                        .IsUnicode(true);

                    b.Property<bool>("IsActive")
                        .HasColumnType("TINYINT(1)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("TINYINT(1)");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(75) CHARACTER SET utf8mb4")
                        .HasMaxLength(75);

                    b.Property<string>("PublicId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("PublicId")
                        .IsUnique();

                    b.ToTable("Credential");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "a.sassani@gmail.com",
                            IsActive = true,
                            IsEmailVerified = true,
                            Password = "$5$10$dGxTx7tECwnupyQxo0iAGN$6EwyCSDswb2JBJUZoKQQUqGCxdkU43SiMszlbglV59k=",
                            PublicId = "41e6655b-cf0b-4c37-8239-07f62f11b266"
                        });
                });

            modelBuilder.Entity("RestApi.Core.Domain.CredentialRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CredentialId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CredentialId");

                    b.HasIndex("RoleId");

                    b.ToTable("CredentialRole");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CredentialId = 1,
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("RestApi.Core.Domain.Logsheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Browser")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CredentialId")
                        .HasColumnType("int");

                    b.Property<string>("IP")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Platform")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CredentialId");

                    b.ToTable("Logsheet");
                });

            modelBuilder.Entity("RestApi.Core.Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = 1
                        },
                        new
                        {
                            Id = 2,
                            Type = 2
                        },
                        new
                        {
                            Id = 3,
                            Type = 3
                        },
                        new
                        {
                            Id = 4,
                            Type = 4
                        });
                });

            modelBuilder.Entity("RestApi.Core.Domain.CredentialRole", b =>
                {
                    b.HasOne("RestApi.Core.Domain.Credential", "Credential")
                        .WithMany("CredentialRole")
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestApi.Core.Domain.Role", "Role")
                        .WithMany("CredentialRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RestApi.Core.Domain.Logsheet", b =>
                {
                    b.HasOne("RestApi.Core.Domain.Client", "Client")
                        .WithMany("Logsheet")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestApi.Core.Domain.Credential", "Credential")
                        .WithMany("Logsheet")
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
