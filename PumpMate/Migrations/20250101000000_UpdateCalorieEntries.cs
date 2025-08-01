using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PumpMate.Migrations
{
    public partial class UpdateCalorieEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntryType",
                table: "CalorieEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Consumed");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryTime",
                table: "CalorieEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryType",
                table: "CalorieEntries");

            migrationBuilder.DropColumn(
                name: "EntryTime",
                table: "CalorieEntries");
        }
    }
} 