using Microsoft.EntityFrameworkCore.Migrations;

namespace DistanceSchool.Data.Migrations
{
    public partial class DelteDisciplineCandidacyRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidacies_Disciplines_DisciplineId",
                table: "Candidacies");

            migrationBuilder.DropIndex(
                name: "IX_Candidacies_DisciplineId",
                table: "Candidacies");

            migrationBuilder.DropColumn(
                name: "DisciplineId",
                table: "Candidacies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisciplineId",
                table: "Candidacies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidacies_DisciplineId",
                table: "Candidacies",
                column: "DisciplineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidacies_Disciplines_DisciplineId",
                table: "Candidacies",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
