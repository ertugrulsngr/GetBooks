using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetBooks.DataAccess.Migrations
{
    public partial class fixCartItemTempForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemTemps_Carts_BookId",
                table: "CartItemTemps");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemTemps_CartId",
                table: "CartItemTemps",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemTemps_Carts_CartId",
                table: "CartItemTemps",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemTemps_Carts_CartId",
                table: "CartItemTemps");

            migrationBuilder.DropIndex(
                name: "IX_CartItemTemps_CartId",
                table: "CartItemTemps");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemTemps_Carts_BookId",
                table: "CartItemTemps",
                column: "BookId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
