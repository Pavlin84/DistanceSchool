using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistanceSchool.Data.Migrations
{
    public partial class SetTwoModelsToDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "StudentLessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "StudentLessons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentLessons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "StudentLessons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "StudentExams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "StudentExams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentExams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "StudentExams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentLessons_IsDeleted",
                table: "StudentLessons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExams_IsDeleted",
                table: "StudentExams",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentLessons_IsDeleted",
                table: "StudentLessons");

            migrationBuilder.DropIndex(
                name: "IX_StudentExams_IsDeleted",
                table: "StudentExams");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "StudentLessons");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "StudentLessons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentLessons");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "StudentLessons");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "StudentExams");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "StudentExams");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentExams");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "StudentExams");
        }
    }
}
