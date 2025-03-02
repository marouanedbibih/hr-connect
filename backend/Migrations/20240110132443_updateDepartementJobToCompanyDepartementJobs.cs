using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class updateDepartementJobToCompanyDepartementJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartementJobs",
                table: "DepartementJobs");

            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "DepartementJobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartementJobs",
                table: "DepartementJobs",
                columns: new[] { "DepartementID", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_DepartementJobs_CompanyId",
                table: "DepartementJobs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartementJobs_Companies_CompanyId",
                table: "DepartementJobs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartementJobs_Companies_CompanyId",
                table: "DepartementJobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartementJobs",
                table: "DepartementJobs");

            migrationBuilder.DropIndex(
                name: "IX_DepartementJobs_CompanyId",
                table: "DepartementJobs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DepartementJobs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartementJobs",
                table: "DepartementJobs",
                columns: new[] { "DepartementID", "JobId" });
        }
    }
}
