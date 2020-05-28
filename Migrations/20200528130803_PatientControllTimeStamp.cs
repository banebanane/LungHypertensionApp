using Microsoft.EntityFrameworkCore.Migrations;

namespace LungHypertensionApp.Migrations
{
    public partial class PatientControllTimeStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeStamp",
                table: "PatientControlls",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "PatientControlls");
        }
    }
}
