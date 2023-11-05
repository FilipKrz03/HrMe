﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(HrMeContext))]
    [Migration("20231105144026_Impr")]
    partial class Impr
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeMonthlyBonus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("BonusAmount")
                        .HasColumnType("float");

                    b.Property<double>("BonusInPercent")
                        .HasColumnType("float");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeesMonthlyBonuses");
                });

            modelBuilder.Entity("Domain.Entities.EmployeePaymentInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ContractType")
                        .HasColumnType("int");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndOfContractDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("HourlyRateBrutto")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartOfContractDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeesPaymentInfos");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeWorkDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EndTimeInMinutesAfterMidnight")
                        .HasColumnType("int");

                    b.Property<int>("StartTimeInMinutesAfterMidnight")
                        .HasColumnType("int");

                    b.Property<DateTime>("WorkDayDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeesWorkDays");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.HasOne("Domain.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeMonthlyBonus", b =>
                {
                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithMany("MonthlyBonus")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Entities.EmployeePaymentInfo", b =>
                {
                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithMany("PaymentInfos")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeWorkDay", b =>
                {
                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithMany("WorkDays")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Entities.Company", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Navigation("MonthlyBonus");

                    b.Navigation("PaymentInfos");

                    b.Navigation("WorkDays");
                });
#pragma warning restore 612, 618
        }
    }
}
