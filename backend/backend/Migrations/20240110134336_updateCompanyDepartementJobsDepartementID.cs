using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class updateCompanyDepartementJobsDepartementID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDepartementJobs_Departements_DepartementID",
                table: "CompanyDepartementJobs");

            migrationBuilder.RenameColumn(
                name: "DepartementID",
                table: "CompanyDepartementJobs",
                newName: "DepartementId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDepartementJobs_Departements_DepartementId",
                table: "CompanyDepartementJobs",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDepartementJobs_Departements_DepartementId",
                table: "CompanyDepartementJobs");

            migrationBuilder.RenameColumn(
                name: "DepartementId",
                table: "CompanyDepartementJobs",
                newName: "DepartementID");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDepartementJobs_Departements_DepartementID",
                table: "CompanyDepartementJobs",
                column: "DepartementID",
                principalTable: "Departements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
