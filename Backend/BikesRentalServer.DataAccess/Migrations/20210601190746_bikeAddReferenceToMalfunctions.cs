using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesRentalServer.DataAccess.Migrations
{
    public partial class bikeAddReferenceToMalfunctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[] { 1, "Wenus", 0 });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[] { 2, "Planeta Małp", 0 });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[] { 3, "Mars", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Status", "Username" },
                values: new object[] { 1, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", 2, 0, "admin" });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Description", "StationId", "Status", "UserId" },
                values: new object[] { 3, null, 1, 0, null });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Description", "StationId", "Status", "UserId" },
                values: new object[] { 4, null, 1, 0, null });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Description", "StationId", "Status", "UserId" },
                values: new object[] { 1, null, 2, 0, null });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Description", "StationId", "Status", "UserId" },
                values: new object[] { 5, null, 2, 0, null });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Description", "StationId", "Status", "UserId" },
                values: new object[] { 2, null, 3, 0, null });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Description", "StationId", "Status", "UserId" },
                values: new object[] { 6, null, 3, 0, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bikes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bikes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bikes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bikes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bikes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Bikes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
