using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorTabledd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_FacultyMembers_FacultyMemberId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "FacultyMemberId",
                table: "Appointments",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_FacultyMemberId",
                table: "Appointments",
                newName: "IX_Appointments_DoctorId");

            migrationBuilder.AddColumn<int>(
                name: "FacultyMemberFacultyId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_FacultyMemberFacultyId",
                table: "Appointments",
                column: "FacultyMemberFacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_FacultyMembers_FacultyMemberFacultyId",
                table: "Appointments",
                column: "FacultyMemberFacultyId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_FacultyMembers_FacultyMemberFacultyId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_FacultyMemberFacultyId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "FacultyMemberFacultyId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Appointments",
                newName: "FacultyMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                newName: "IX_Appointments_FacultyMemberId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Days",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_FacultyMembers_FacultyMemberId",
                table: "Appointments",
                column: "FacultyMemberId",
                principalTable: "FacultyMembers",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
