using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class bookedAppointmentk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedAppointments_Assistants_AssistantId",
                table: "BookedAppointments");

            migrationBuilder.DropIndex(
                name: "IX_BookedAppointments_AssistantId",
                table: "BookedAppointments");

            migrationBuilder.DropColumn(
                name: "AssistantId",
                table: "BookedAppointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantId",
                table: "BookedAppointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookedAppointments_AssistantId",
                table: "BookedAppointments",
                column: "AssistantId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedAppointments_Assistants_AssistantId",
                table: "BookedAppointments",
                column: "AssistantId",
                principalTable: "Assistants",
                principalColumn: "AssistantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
