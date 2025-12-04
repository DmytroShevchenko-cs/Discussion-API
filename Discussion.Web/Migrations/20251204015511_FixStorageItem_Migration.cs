using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discussion.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixStorageItem_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_id",
                table: "comment_images");

            migrationBuilder.DropColumn(
                name: "file_id",
                table: "comment_files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "file_id",
                table: "comment_images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "file_id",
                table: "comment_files",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
