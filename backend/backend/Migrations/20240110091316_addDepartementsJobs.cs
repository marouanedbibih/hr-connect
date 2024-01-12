using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class addDepartementsJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Departements_DepartementId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_DepartementId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "DepartementId",
                table: "Jobs");

            migrationBuilder.CreateTable(
                name: "DepartementJobs",
                columns: table => new
                {
                    DepartementID = table.Column<long>(type: "bigint", nullable: false),
                    JobId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartementJobs", x => new { x.DepartementID, x.JobId });
                    table.ForeignKey(
                        name: "FK_DepartementJobs_Departements_DepartementID",
                        column: x => x.DepartementID,
                        principalTable: "Departements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartementJobs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartementJobs_JobId",
                table: "DepartementJobs",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartementJobs");

            migrationBuilder.AddColumn<long>(
                name: "DepartementId",
                table: "Jobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DepartementId",
                table: "Jobs",
                column: "DepartementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Departements_DepartementId",
                table: "Jobs",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
