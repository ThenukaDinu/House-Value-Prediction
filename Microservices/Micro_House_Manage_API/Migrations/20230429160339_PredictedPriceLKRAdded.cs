using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro_House_Manage_API.Migrations
{
    public partial class PredictedPriceLKRAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PredictedPriceLKR",
                table: "Houses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredictedPriceLKR",
                table: "Houses");
        }
    }
}
