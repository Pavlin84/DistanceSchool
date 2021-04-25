using Microsoft.EntityFrameworkCore.Migrations;

namespace DistanceSchool.Data.Migrations
{
    public partial class AddAddressProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Schools");
        }
    }
}
