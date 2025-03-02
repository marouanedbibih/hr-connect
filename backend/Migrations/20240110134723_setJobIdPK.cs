using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class setJobIdPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyDepartementJobs",
                table: "CompanyDepartementJobs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyDepartementJobs",
                table: "CompanyDepartementJobs",
                columns: new[] { "DepartementId", "CompanyId", "JobId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyDepartementJobs",
                table: "CompanyDepartementJobs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyDepartementJobs",
                table: "CompanyDepartementJobs",
                columns: new[] { "DepartementId", "CompanyId" });
        }
    }
}
