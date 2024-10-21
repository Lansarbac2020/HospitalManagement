using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class seedAssistantData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "AvailableBeds", "DepartmentName", "PatientCount" },
                values: new object[,]
                {
                    { 1, 0, "Pediatric Emergency", 0 },
                    { 2, 0, "Pediatric Intensive Care", 0 }
                });

            migrationBuilder.InsertData(
                table: "Assistants",
                columns: new[] { "AssistantId", "Address", "DepartmentId", "Email", "FirstName", "LastName", "Phone", "ShiftEndTime", "ShiftStartTime" },
                values: new object[,]
                {
                    { 1, "123 Elm St, Springfield", 1, "john.doe@example.com", "John", "Doe", "555-1234", new DateTime(2023, 10, 21, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 21, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "456 Oak St, Springfield", 1, "jane.smith@example.com", "Jane", "Smith", "555-5678", new DateTime(2023, 10, 21, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 21, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "789 Pine St, Springfield", 2, "emily.johnson@example.com", "Emily", "Johnson", "555-8765", new DateTime(2023, 10, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 21, 10, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assistants",
                keyColumn: "AssistantId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assistants",
                keyColumn: "AssistantId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Assistants",
                keyColumn: "AssistantId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2);
        }
    }
}
