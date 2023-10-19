using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_computing_project_LAST.Data.Migrations
{
    /// <inheritdoc />
    public partial class addPriceToCafe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Cafe",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cafe");
        }
    }
}
