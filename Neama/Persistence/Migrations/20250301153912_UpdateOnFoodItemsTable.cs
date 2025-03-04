using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neama.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOnFoodItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItem_Businesses_BusinessId",
                table: "FoodItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_FoodItem_FoodItemId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodItem",
                table: "FoodItem");

            migrationBuilder.RenameTable(
                name: "FoodItem",
                newName: "FoodItems");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItem_BusinessId",
                table: "FoodItems",
                newName: "IX_FoodItems_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodItems",
                table: "FoodItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Businesses_BusinessId",
                table: "FoodItems",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_FoodItems_FoodItemId",
                table: "Order",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Businesses_BusinessId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_FoodItems_FoodItemId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodItems",
                table: "FoodItems");

            migrationBuilder.RenameTable(
                name: "FoodItems",
                newName: "FoodItem");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_BusinessId",
                table: "FoodItem",
                newName: "IX_FoodItem_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodItem",
                table: "FoodItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItem_Businesses_BusinessId",
                table: "FoodItem",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_FoodItem_FoodItemId",
                table: "Order",
                column: "FoodItemId",
                principalTable: "FoodItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
