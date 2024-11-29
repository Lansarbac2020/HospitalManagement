using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class BookedTabledds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyMembers_Departments_DepartmentId1",
                table: "FacultyMembers");

            migrationBuilder.DropIndex(
                name: "IX_FacultyMembers_DepartmentId1",
                table: "FacultyMembers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "FacultyMembers");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "FacultyMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "FacultyMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "FacultyMembers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacultyMembers_DepartmentId1",
                table: "FacultyMembers",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyMembers_Departments_DepartmentId1",
                table: "FacultyMembers",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }
    }
}
