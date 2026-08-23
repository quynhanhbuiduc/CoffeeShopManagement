using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaféPourLaVie.Migrations
{
    /// <inheritdoc />
    public partial class AddImportStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ImportReceipts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ImportReceipts");
        }
    }
}
