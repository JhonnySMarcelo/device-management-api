using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class addindexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Devices_Brand",
                table: "Devices",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_State",
                table: "Devices",
                column: "State");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_Brand",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_State",
                table: "Devices");
        }
    }
}
