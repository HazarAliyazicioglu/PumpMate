﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PumpMate.Migrations
{
    public partial class AddExercisesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDetails_Exercise_ExerciseId",
                table: "WorkoutDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise");

            migrationBuilder.RenameTable(
                name: "Exercise",
                newName: "Exercises");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDetails_Exercises_ExerciseId",
                table: "WorkoutDetails",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDetails_Exercises_ExerciseId",
                table: "WorkoutDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "Exercise");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDetails_Exercise_ExerciseId",
                table: "WorkoutDetails",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
