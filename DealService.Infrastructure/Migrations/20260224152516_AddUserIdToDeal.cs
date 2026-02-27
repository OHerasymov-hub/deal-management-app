using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToDeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Deals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Deals");
        }
    }
}
