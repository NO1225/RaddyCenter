using Microsoft.EntityFrameworkCore.Migrations;

namespace RaddyCenter.Data.Migrations
{
    public partial class Addingavailablecolumntobookstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Available",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "Books");
        }
    }
}
