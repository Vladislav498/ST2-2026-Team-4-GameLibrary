using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST2_2026_Team_4_GameLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ImageUploadFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverImage",
                table: "Games",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Games",
                newName: "CoverImage");
        }
    }
}
