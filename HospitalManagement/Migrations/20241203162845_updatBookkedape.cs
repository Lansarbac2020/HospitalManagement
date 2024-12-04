using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class updatBookkedape : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacultyMemberFacultyId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "FacultyMemberFacultyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "FacultyMemberFacultyId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FacultyMemberFacultyId",
                table: "Departments",
                column: "FacultyMemberFacultyId",
                unique: true,
                filter: "[FacultyMemberFacultyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberFacultyId",
                table: "Departments",
                column: "FacultyMemberFacultyId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberFacultyId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_FacultyMemberFacultyId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "FacultyMemberFacultyId",
                table: "Departments");
        }
    }
}
