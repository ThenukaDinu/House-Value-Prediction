using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro_House_Manage_API.Migrations
{
    public partial class updatehousestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ListingPrice",
                table: "Listings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "A1stFlrSF",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FullBath",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GarageArea",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GarageCars",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrLivArea",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverallQual",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotRmsAbvGrd",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalBsmtSF",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearBuilt",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearRemodAdd",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingPrice",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "A1stFlrSF",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "FullBath",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "GarageArea",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "GarageCars",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "GrLivArea",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "OverallQual",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "TotRmsAbvGrd",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "TotalBsmtSF",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "YearBuilt",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "YearRemodAdd",
                table: "Houses");
        }
    }
}
