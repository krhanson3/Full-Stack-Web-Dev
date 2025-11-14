using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2025_Project3_krhanson3.Data.Migrations
{
    /// <inheritdoc />
    public partial class SRIHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IMDBUrlSRI",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterSRI",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IMDBUrlSRI",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoSRI",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMDBUrlSRI",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "PosterSRI",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IMDBUrlSRI",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "PhotoSRI",
                table: "Actors");
        }
    }
}
