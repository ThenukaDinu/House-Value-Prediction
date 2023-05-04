using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro_House_Manage_API.Migrations
{
    public partial class housePhotosReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HouseId",
                table: "HousePhotos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HousePhotos_HouseId",
                table: "HousePhotos",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_HousePhotos_Houses_HouseId",
                table: "HousePhotos",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousePhotos_Houses_HouseId",
                table: "HousePhotos");

            migrationBuilder.DropIndex(
                name: "IX_HousePhotos_HouseId",
                table: "HousePhotos");

            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "HousePhotos");
        }
    }
}
