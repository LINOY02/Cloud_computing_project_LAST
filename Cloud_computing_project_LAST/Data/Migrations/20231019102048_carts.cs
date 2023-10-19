using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_computing_project_LAST.Data.Migrations
{
    /// <inheritdoc />
    public partial class carts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Cafe_CafeId",
                table: "Cart");

            migrationBuilder.AlterColumn<int>(
                name: "CafeId",
                table: "Cart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Cafe_CafeId",
                table: "Cart",
                column: "CafeId",
                principalTable: "Cafe",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Cafe_CafeId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Cart");

            migrationBuilder.AlterColumn<int>(
                name: "CafeId",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Cafe_CafeId",
                table: "Cart",
                column: "CafeId",
                principalTable: "Cafe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
