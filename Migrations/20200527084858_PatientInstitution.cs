using Microsoft.EntityFrameworkCore.Migrations;

namespace LungHypertensionApp.Migrations
{
    public partial class PatientInstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstitutionId",
                table: "PatientBaseData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientBaseData_InstitutionId",
                table: "PatientBaseData",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientBaseData_Institutions_InstitutionId",
                table: "PatientBaseData",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientBaseData_Institutions_InstitutionId",
                table: "PatientBaseData");

            migrationBuilder.DropIndex(
                name: "IX_PatientBaseData_InstitutionId",
                table: "PatientBaseData");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "PatientBaseData");
        }
    }
}
