using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyDepartementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departements_Companies_CompanyID",
                table: "Departements");

            migrationBuilder.DropIndex(
                name: "IX_Departements_CompanyID",
                table: "Departements");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Departements");

            migrationBuilder.CreateTable(
                name: "CompanyDepartments",
                columns: table => new
                {
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDepartments", x => new { x.CompanyId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_CompanyDepartments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyDepartments_Departements_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDepartments_DepartmentId",
                table: "CompanyDepartments",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDepartments");

            migrationBuilder.AddColumn<long>(
                name: "CompanyID",
                table: "Departements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Departements_CompanyID",
                table: "Departements",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Departements_Companies_CompanyID",
                table: "Departements",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
