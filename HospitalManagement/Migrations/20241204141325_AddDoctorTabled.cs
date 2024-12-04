using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorTabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_FacultyMembers_DepartmentHeadId",
                table: "Doctors");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_FacultyMembers_DepartmentHeadId",
                table: "Doctors",
                column: "DepartmentHeadId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_FacultyMembers_DepartmentHeadId",
                table: "Doctors");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_FacultyMembers_DepartmentHeadId",
                table: "Doctors",
                column: "DepartmentHeadId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
