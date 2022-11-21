using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class ModifyCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Discount_DiscountCode",
                table: "Discount",
                column: "DiscountCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Discount_DiscountCode",
                table: "Discount");
        }
    }
}
