using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_computing_project_LAST.Data.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Cafe_CafeId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_CafeId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "CafeId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Cart",
                newName: "TotalPrice");

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InStock = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsersInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZIPCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "UsersInfo");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Cart",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "CafeId",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_CafeId",
                table: "Cart",
                column: "CafeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Cafe_CafeId",
                table: "Cart",
                column: "CafeId",
                principalTable: "Cafe",
                principalColumn: "Id");
        }
    }
}
