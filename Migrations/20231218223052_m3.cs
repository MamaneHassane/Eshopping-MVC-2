using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopping_MVC.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductCopies_ProductId",
                table: "ProductCopies",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCopies_Products_ProductId",
                table: "ProductCopies",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "productId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCopies_Products_ProductId",
                table: "ProductCopies");

            migrationBuilder.DropIndex(
                name: "IX_ProductCopies_ProductId",
                table: "ProductCopies");
        }
    }
}
