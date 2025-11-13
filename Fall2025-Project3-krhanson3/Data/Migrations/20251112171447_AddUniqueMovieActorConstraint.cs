using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2025_Project3_krhanson3.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueMovieActorConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovieActor_MovieId",
                table: "MovieActor");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActor_MovieId_ActorId",
                table: "MovieActor",
                columns: new[] { "MovieId", "ActorId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovieActor_MovieId_ActorId",
                table: "MovieActor");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActor_MovieId",
                table: "MovieActor",
                column: "MovieId");
        }
    }
}
