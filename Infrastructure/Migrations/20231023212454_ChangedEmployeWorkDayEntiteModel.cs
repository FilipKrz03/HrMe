using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEmployeWorkDayEntiteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "EmployeesWorkDays");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "EmployeesWorkDays");

            migrationBuilder.AddColumn<int>(
                name: "EndTimeInMinutesAfterMidnight",
                table: "EmployeesWorkDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartTimeInMinutesAfterMidnight",
                table: "EmployeesWorkDays",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTimeInMinutesAfterMidnight",
                table: "EmployeesWorkDays");

            migrationBuilder.DropColumn(
                name: "StartTimeInMinutesAfterMidnight",
                table: "EmployeesWorkDays");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "EmployeesWorkDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "EmployeesWorkDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
