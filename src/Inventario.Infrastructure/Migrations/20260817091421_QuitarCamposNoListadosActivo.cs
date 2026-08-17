using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuitarCamposNoListadosActivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "activos");

            migrationBuilder.DropColumn(
                name: "TipoActivo",
                table: "activos");

            migrationBuilder.DropColumn(
                name: "ubicacion_latitud",
                table: "activos");

            migrationBuilder.DropColumn(
                name: "ubicacion_longitud",
                table: "activos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "activos",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TipoActivo",
                table: "activos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "ubicacion_latitud",
                table: "activos",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ubicacion_longitud",
                table: "activos",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: true);
        }
    }
}
