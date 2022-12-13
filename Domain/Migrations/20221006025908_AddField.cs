using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Domain.Migrations
{
    public partial class AddField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartItem");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Review",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Review",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Payment",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Discount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "AppUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "AppUser");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Review",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Review",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Product",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Product",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "CartItem",
                type: "DECIMAL",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
