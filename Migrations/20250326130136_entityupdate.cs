using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NepHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class entityupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Entity",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Entity",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Entity");
        }
    }
}
