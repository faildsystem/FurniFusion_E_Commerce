using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniFusion.Migrations
{
    /// <inheritdoc />
    public partial class RmoveOrderItemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "item_id",
            table: "Order_Item");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
