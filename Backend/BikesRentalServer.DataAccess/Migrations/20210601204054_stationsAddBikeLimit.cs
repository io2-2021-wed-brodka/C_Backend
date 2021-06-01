using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesRentalServer.DataAccess.Migrations
{
    public partial class stationsAddBikeLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BikeLimit",
                table: "Stations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BikeLimit",
                table: "Stations");
        }
    }
}
