using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceWatcher.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlinerKeyToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnlinerId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "OnlinerKey",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnlinerKey",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "OnlinerId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
