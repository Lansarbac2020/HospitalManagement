using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class updateAdminDashView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberId",
                table: "Departments");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberId",
                table: "Departments",
                column: "FacultyMemberId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberId",
                table: "Departments");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberId",
                table: "Departments",
                column: "FacultyMemberId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
