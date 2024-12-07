using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class shiftMigrattiond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Emergencies");

            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Emergencies",
                newName: "DateCreated");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Emergencies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Emergencies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Emergencies");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Emergencies");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Emergencies",
                newName: "DatePosted");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Emergencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
