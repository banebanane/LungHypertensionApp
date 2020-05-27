using Microsoft.EntityFrameworkCore.Migrations;

namespace LungHypertensionApp.Migrations
{
    public partial class PatientsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientControlls_PatientBaseData_PatientId",
                table: "PatientControlls");

            migrationBuilder.DropTable(
                name: "PatientBaseData");

            migrationBuilder.DropTable(
                name: "PatientContactData");

            migrationBuilder.DropTable(
                name: "PatientDiagnostic");

            migrationBuilder.DropTable(
                name: "PatientFunctional");

            migrationBuilder.DropTable(
                name: "PatientLab");

            migrationBuilder.DropTable(
                name: "PatientTheraphy");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<long>(nullable: false),
                    InstitutionId = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EKG = table.Column<string>(nullable: true),
                    Risk = table.Column<string>(nullable: true),
                    WHO = table.Column<string>(nullable: true),
                    NtProBnp = table.Column<double>(nullable: false),
                    Hgb = table.Column<double>(nullable: false),
                    Hct = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InstitutionId",
                table: "Patients",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientControlls_Patients_PatientId",
                table: "PatientControlls",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientControlls_Patients_PatientId",
                table: "PatientControlls");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.CreateTable(
                name: "PatientBaseData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstitutionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientBaseData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientBaseData_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientContactData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientContactData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientDiagnostic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Risk = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDiagnostic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientFunctional",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NtProBnp = table.Column<double>(type: "float", nullable: false),
                    WHO = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFunctional", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientLab",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hct = table.Column<double>(type: "float", nullable: false),
                    Hgb = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLab", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientTheraphy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientTheraphy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientBaseData_InstitutionId",
                table: "PatientBaseData",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientControlls_PatientBaseData_PatientId",
                table: "PatientControlls",
                column: "PatientId",
                principalTable: "PatientBaseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
