using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class updatBookkedap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Assistants_AssistantId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AssistantId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AssistantId",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AssistantId",
                table: "Appointments",
                column: "AssistantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Assistants_AssistantId",
                table: "Appointments",
                column: "AssistantId",
                principalTable: "Assistants",
                principalColumn: "AssistantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
