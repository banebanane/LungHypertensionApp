using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LungHypertensionApp.Migrations
{
    public partial class PatientTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientBaseData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientBaseData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientContactData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientContactData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientDiagnostic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EKG = table.Column<string>(nullable: true),
                    Risk = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDiagnostic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientFunctional",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WHO = table.Column<string>(nullable: true),
                    NtProBnp = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFunctional", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientLab",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hgb = table.Column<double>(nullable: false),
                    Hct = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLab", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientTheraphy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientTheraphy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientControlls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientIdId = table.Column<int>(nullable: true),
                    ControllDate = table.Column<DateTime>(nullable: false),
                    WeekHearth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientControlls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientControlls_PatientBaseData_PatientIdId",
                        column: x => x.PatientIdId,
                        principalTable: "PatientBaseData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientControlls_PatientIdId",
                table: "PatientControlls",
                column: "PatientIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientContactData");

            migrationBuilder.DropTable(
                name: "PatientControlls");

            migrationBuilder.DropTable(
                name: "PatientDiagnostic");

            migrationBuilder.DropTable(
                name: "PatientFunctional");

            migrationBuilder.DropTable(
                name: "PatientLab");

            migrationBuilder.DropTable(
                name: "PatientTheraphy");

            migrationBuilder.DropTable(
                name: "PatientBaseData");
        }
    }
}
