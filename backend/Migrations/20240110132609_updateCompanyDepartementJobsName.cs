using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class updateCompanyDepartementJobsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartementJobs_Companies_CompanyId",
                table: "DepartementJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartementJobs_Departements_DepartementID",
                table: "DepartementJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartementJobs_Jobs_JobId",
                table: "DepartementJobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartementJobs",
                table: "DepartementJobs");

            migrationBuilder.RenameTable(
                name: "DepartementJobs",
                newName: "CompanyDepartementJobs");

            migrationBuilder.RenameIndex(
                name: "IX_DepartementJobs_JobId",
                table: "CompanyDepartementJobs",
                newName: "IX_CompanyDepartementJobs_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartementJobs_CompanyId",
                table: "CompanyDepartementJobs",
                newName: "IX_CompanyDepartementJobs_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyDepartementJobs",
                table: "CompanyDepartementJobs",
                columns: new[] { "DepartementID", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDepartementJobs_Companies_CompanyId",
                table: "CompanyDepartementJobs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDepartementJobs_Departements_DepartementID",
                table: "CompanyDepartementJobs",
                column: "DepartementID",
                principalTable: "Departements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDepartementJobs_Jobs_JobId",
                table: "CompanyDepartementJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDepartementJobs_Companies_CompanyId",
                table: "CompanyDepartementJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDepartementJobs_Departements_DepartementID",
                table: "CompanyDepartementJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDepartementJobs_Jobs_JobId",
                table: "CompanyDepartementJobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyDepartementJobs",
                table: "CompanyDepartementJobs");

            migrationBuilder.RenameTable(
                name: "CompanyDepartementJobs",
                newName: "DepartementJobs");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyDepartementJobs_JobId",
                table: "DepartementJobs",
                newName: "IX_DepartementJobs_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyDepartementJobs_CompanyId",
                table: "DepartementJobs",
                newName: "IX_DepartementJobs_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartementJobs",
                table: "DepartementJobs",
                columns: new[] { "DepartementID", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DepartementJobs_Companies_CompanyId",
                table: "DepartementJobs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartementJobs_Departements_DepartementID",
                table: "DepartementJobs",
                column: "DepartementID",
                principalTable: "Departements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartementJobs_Jobs_JobId",
                table: "DepartementJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
