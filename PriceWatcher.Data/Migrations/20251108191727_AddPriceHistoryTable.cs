using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceWatcher.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OnlinerKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldPriceMin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NewPriceMin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OldPriceMax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NewPriceMax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceHistories");
        }
    }
}
