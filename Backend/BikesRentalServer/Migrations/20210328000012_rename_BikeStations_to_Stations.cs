using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesRentalServer.Migrations
{
    public partial class rename_BikeStations_to_Stations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bikes_BikeStations_StationId",
                table: "Bikes");

            migrationBuilder.DropTable(
                name: "BikeStations");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Bikes",
                newName: "Status");

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bikes_Stations_StationId",
                table: "Bikes",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bikes_Stations_StationId",
                table: "Bikes");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Bikes",
                newName: "State");

            migrationBuilder.CreateTable(
                name: "BikeStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BikeStations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bikes_BikeStations_StationId",
                table: "Bikes",
                column: "StationId",
                principalTable: "BikeStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
