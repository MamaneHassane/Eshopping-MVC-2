using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopping_MVC.Migrations
{
    public partial class m4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartProductIds",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Clients",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "CartProductIds",
                table: "Carts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
