using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniFusion.Migrations
{
    /// <inheritdoc />
    public partial class removePriceDiscounted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceDiscounted",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceDiscounted",
                table: "Product",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
