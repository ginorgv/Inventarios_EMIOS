using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSensorIdAComponente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "componentes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_componentes_SensorId",
                table: "componentes",
                column: "SensorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_componentes_SensorId",
                table: "componentes");

            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "componentes");
        }
    }
}
