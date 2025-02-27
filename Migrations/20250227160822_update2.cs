using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NepHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpdateRequests_AspNetUsers_UserId1",
                table: "UpdateRequests");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "UpdateRequests",
                newName: "RequestById");

            migrationBuilder.RenameIndex(
                name: "IX_UpdateRequests_UserId1",
                table: "UpdateRequests",
                newName: "IX_UpdateRequests_RequestById");

            migrationBuilder.AddForeignKey(
                name: "FK_UpdateRequests_AspNetUsers_RequestById",
                table: "UpdateRequests",
                column: "RequestById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpdateRequests_AspNetUsers_RequestById",
                table: "UpdateRequests");

            migrationBuilder.RenameColumn(
                name: "RequestById",
                table: "UpdateRequests",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_UpdateRequests_RequestById",
                table: "UpdateRequests",
                newName: "IX_UpdateRequests_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UpdateRequests_AspNetUsers_UserId1",
                table: "UpdateRequests",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
