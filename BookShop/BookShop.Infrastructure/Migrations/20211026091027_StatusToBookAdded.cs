using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShop.Infrastructure.Migrations
{
    public partial class StatusToBookAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Novelty",
                schema: "BookShop",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "BookShop",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "BookShop",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "Novelty",
                schema: "BookShop",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
