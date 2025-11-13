using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2025_Project3_krhanson3.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "Movies",
                newName: "Poster");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Poster",
                table: "Movies",
                newName: "Photo");
        }
    }
}
