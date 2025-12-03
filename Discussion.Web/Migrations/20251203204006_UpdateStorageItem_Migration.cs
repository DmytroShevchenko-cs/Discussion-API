using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discussion.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStorageItem_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "original_name",
                table: "storage_items",
                newName: "base_name");

            migrationBuilder.AddColumn<string>(
                name: "item_hash",
                table: "storage_items",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "item_hash",
                table: "storage_items");

            migrationBuilder.RenameColumn(
                name: "base_name",
                table: "storage_items",
                newName: "original_name");
        }
    }
}
