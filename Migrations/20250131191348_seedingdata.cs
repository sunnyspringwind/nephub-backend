using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NepHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedingdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Richest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NetWorth = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Richest", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Richest",
                columns: new[] { "Id", "Designation", "Image", "Name", "NetWorth" },
                values: new object[,]
                {
                    { 1, "Chairman of Chaudhary Group", "https://sgp1.digitaloceanspaces.com/awe/publication-nepalaya/persons/binod_chaudhary.jpg", "Binod Chaudhary", "1.4 Billion USD" },
                    { 2, "CEO of Melbourne Institute of Technology", "https://republicaimg.nagariknewscdn.com/shared/web/uploads/media/shesh-ghale.jpg", "Shesh Ghale", "1.2 Billion USD" },
                    { 3, "Co-founder of Melbourne Institute of Technology", "https://biographnepal.com/wp-content/uploads/2024/09/Jamuna-Gurung-e1727365992412.jpg", "Jamuna Gurung", "1.2 Billion USD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Richest");
        }
    }
}
