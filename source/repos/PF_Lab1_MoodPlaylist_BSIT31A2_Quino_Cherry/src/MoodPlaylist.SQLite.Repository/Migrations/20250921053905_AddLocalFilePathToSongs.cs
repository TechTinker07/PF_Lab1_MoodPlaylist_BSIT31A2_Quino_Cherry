using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodPlaylist.SQLite.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalFilePathToSongs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalFilePath",
                table: "Songs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalFilePath",
                table: "Songs");
        }
    }
}
