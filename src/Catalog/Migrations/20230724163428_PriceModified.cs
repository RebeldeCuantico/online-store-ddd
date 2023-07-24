using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Migrations
{
    /// <inheritdoc />
    public partial class PriceModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "catalog",
                table: "Product",
                newName: "Amount");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "catalog",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "catalog",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "catalog",
                table: "Product",
                newName: "Price");
        }
    }
}
