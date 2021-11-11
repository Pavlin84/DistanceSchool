using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistanceSchool.Data.Migrations
{
    public partial class SetDisciplineTeachetToDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "DisciplineTeachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "DisciplineTeachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DisciplineTeachers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "DisciplineTeachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineTeachers_IsDeleted",
                table: "DisciplineTeachers",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DisciplineTeachers_IsDeleted",
                table: "DisciplineTeachers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "DisciplineTeachers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "DisciplineTeachers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DisciplineTeachers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "DisciplineTeachers");
        }
    }
}
