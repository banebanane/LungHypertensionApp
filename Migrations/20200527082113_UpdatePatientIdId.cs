using Microsoft.EntityFrameworkCore.Migrations;

namespace LungHypertensionApp.Migrations
{
    public partial class UpdatePatientIdId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientControlls_PatientBaseData_PatientIdId",
                table: "PatientControlls");

            migrationBuilder.DropIndex(
                name: "IX_PatientControlls_PatientIdId",
                table: "PatientControlls");

            migrationBuilder.DropColumn(
                name: "PatientIdId",
                table: "PatientControlls");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "PatientControlls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientControlls_PatientId",
                table: "PatientControlls",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientControlls_PatientBaseData_PatientId",
                table: "PatientControlls",
                column: "PatientId",
                principalTable: "PatientBaseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientControlls_PatientBaseData_PatientId",
                table: "PatientControlls");

            migrationBuilder.DropIndex(
                name: "IX_PatientControlls_PatientId",
                table: "PatientControlls");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "PatientControlls");

            migrationBuilder.AddColumn<int>(
                name: "PatientIdId",
                table: "PatientControlls",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientControlls_PatientIdId",
                table: "PatientControlls",
                column: "PatientIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientControlls_PatientBaseData_PatientIdId",
                table: "PatientControlls",
                column: "PatientIdId",
                principalTable: "PatientBaseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
