﻿// <auto-generated />
using System;
using EnsekCodingExercise.ApiService.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EnsekCodingExercise.ApiService.Migrations
{
    [DbContext(typeof(CustomerContext))]
    [Migration("20240704211806_UpdateColumnRestrictions")]
    partial class UpdateColumnRestrictions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EnsekCodingExercise.ApiService.Models.Database.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("EnsekCodingExercise.ApiService.Models.Database.Reading", b =>
                {
                    b.Property<int>("ReadingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReadingId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("MeterReadValue")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime?>("ReadingDateTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.HasKey("ReadingId");

                    b.HasIndex("AccountId");

                    b.ToTable("Readings");
                });

            modelBuilder.Entity("EnsekCodingExercise.ApiService.Models.Database.Reading", b =>
                {
                    b.HasOne("EnsekCodingExercise.ApiService.Models.Database.Account", null)
                        .WithMany("Readings")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EnsekCodingExercise.ApiService.Models.Database.Account", b =>
                {
                    b.Navigation("Readings");
                });
#pragma warning restore 612, 618
        }
    }
}
