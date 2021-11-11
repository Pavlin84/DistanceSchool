using Microsoft.EntityFrameworkCore.Migrations;

namespace DistanceSchool.Data.Migrations
{
    public partial class RenameProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImagePath",
                table: "AspNetUsers",
                newName: "ProfileImageExtension");

            migrationBuilder.RenameColumn(
                name: "ApplicationDocumentsPath",
                table: "AspNetUsers",
                newName: "ApplicationDocumentsExtension");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageExtension",
                table: "AspNetUsers",
                newName: "ProfileImagePath");

            migrationBuilder.RenameColumn(
                name: "ApplicationDocumentsExtension",
                table: "AspNetUsers",
                newName: "ApplicationDocumentsPath");
        }
    }
}
