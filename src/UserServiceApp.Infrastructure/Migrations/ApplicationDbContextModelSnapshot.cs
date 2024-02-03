﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserServiceApp.Infrastructure.Persistance;

#nullable disable

namespace UserServiceApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserServiceApp.Domain.UsersAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id", "Email");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8b708975-1493-4201-9101-d614d50c64df"),
                            Culture = "en-US",
                            DateCreated = new DateTime(2024, 2, 2, 17, 8, 5, 322, DateTimeKind.Utc).AddTicks(7006),
                            DateModified = new DateTime(2024, 2, 2, 17, 8, 5, 322, DateTimeKind.Utc).AddTicks(7006),
                            Email = "admin@localhost",
                            FullName = "Admin",
                            IsAdmin = true,
                            Language = "English",
                            MobileNumber = "+65467891324586",
                            Password = "*****************",
                            PasswordHash = "$2a$11$66w6KHRgEJLxQExVzMZfF.8RLuXeDg9XT4KgXaMfD/cO0/5a0JQtW",
                            Username = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
