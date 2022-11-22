using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class ChangeDecimalValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountValue",
                table: "Discount",
                type: "DECIMAL(10,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountValue",
                table: "Discount",
                type: "DECIMAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(10,5)");
        }
    }
}
