using Microsoft.EntityFrameworkCore.Migrations;

namespace DistanceSchool.Data.Migrations
{
    public partial class CreateRelationSchoomManagerApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Schools",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_ManagerId",
                table: "Schools",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_AspNetUsers_ManagerId",
                table: "Schools",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schools_AspNetUsers_ManagerId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_ManagerId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Schools");
        }
    }
}
