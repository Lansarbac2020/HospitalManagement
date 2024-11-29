using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class BookedTabledd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyMembers_Departments_DepartmentId",
                table: "FacultyMembers");

            migrationBuilder.DropIndex(
                name: "IX_FacultyMembers_DepartmentId",
                table: "FacultyMembers");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "FacultyMembers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacultyMemberId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "FacultyMemberId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "FacultyMemberId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_FacultyMembers_DepartmentId1",
                table: "FacultyMembers",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FacultyMemberId",
                table: "Departments",
                column: "FacultyMemberId",
                unique: true,
                filter: "[FacultyMemberId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberId",
                table: "Departments",
                column: "FacultyMemberId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyMembers_Departments_DepartmentId1",
                table: "FacultyMembers",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_FacultyMembers_FacultyMemberId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultyMembers_Departments_DepartmentId1",
                table: "FacultyMembers");

            migrationBuilder.DropIndex(
                name: "IX_FacultyMembers_DepartmentId1",
                table: "FacultyMembers");

            migrationBuilder.DropIndex(
                name: "IX_Departments_FacultyMemberId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "FacultyMembers");

            migrationBuilder.DropColumn(
                name: "FacultyMemberId",
                table: "Departments");

            migrationBuilder.CreateIndex(
                name: "IX_FacultyMembers_DepartmentId",
                table: "FacultyMembers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyMembers_Departments_DepartmentId",
                table: "FacultyMembers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
