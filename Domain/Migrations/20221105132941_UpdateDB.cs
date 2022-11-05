using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class UpdateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_AppUser_UserId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AppUser_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Discount_DiscountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AppUser_UserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_AppUser_UserId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_AppUser_UserId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AppUser_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_AppUser_UserId",
                table: "UserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_WishListItem_AppUser_UserId",
                table: "WishListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CartItem");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "WishListItemId",
                table: "WishListItem",
                newName: "WishItemId");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Review",
                newName: "ReviewItemId");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Category",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Brand",
                newName: "Image");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_User_UserId",
                table: "CartItem",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Discount_DiscountId",
                table: "Order",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_User_UserId",
                table: "UserClaims",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_User_UserId",
                table: "UserLogins",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_User_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_User_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItem_User_UserId",
                table: "WishListItem",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_User_UserId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Discount_DiscountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_User_UserId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_User_UserId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_User_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_User_UserId",
                table: "UserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_WishListItem_User_UserId",
                table: "WishListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "AppUser");

            migrationBuilder.RenameColumn(
                name: "WishItemId",
                table: "WishListItem",
                newName: "WishListItemId");

            migrationBuilder.RenameColumn(
                name: "ReviewItemId",
                table: "Review",
                newName: "ReviewId");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Category",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Brand",
                newName: "ImagePath");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "OrderItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "CartItem",
                type: "DECIMAL",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_AppUser_UserId",
                table: "CartItem",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AppUser_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Discount_DiscountId",
                table: "Order",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AppUser_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_AppUser_UserId",
                table: "UserClaims",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_AppUser_UserId",
                table: "UserLogins",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AppUser_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_AppUser_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItem_AppUser_UserId",
                table: "WishListItem",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}