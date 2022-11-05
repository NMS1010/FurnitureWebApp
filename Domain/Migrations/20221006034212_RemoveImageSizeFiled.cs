using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class RemoveImageSizeFiled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "ImageSize",
                table: "Brand");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "ProductImage",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ImageSize",
                table: "Brand",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
