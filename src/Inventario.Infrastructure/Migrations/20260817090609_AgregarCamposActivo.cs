using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposActivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EficienciaPct",
                table: "activos",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinGarantia",
                table: "activos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PotenciaNominalKw",
                table: "activos",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProximaRevision",
                table: "activos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaRevision",
                table: "activos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Usuario = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadTipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    TipoPermiso = table.Column<int>(type: "int", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreadoPor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModificadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModificadoPor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permisos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_permisos_Usuario_EntidadTipo_EntidadId",
                table: "permisos",
                columns: new[] { "Usuario", "EntidadTipo", "EntidadId" },
                unique: true);

            // Renombrar los valores de estado guardados como texto al nuevo vocabulario.
            migrationBuilder.Sql("UPDATE activos SET Estado = 'Operativo' WHERE Estado = 'Activo'");
            migrationBuilder.Sql("UPDATE activos SET Estado = 'Mantenimiento' WHERE Estado = 'EnMantenimiento'");
            migrationBuilder.Sql("UPDATE activos SET Estado = 'Inactivo' WHERE Estado = 'Baja'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir el vocabulario de estado.
            migrationBuilder.Sql("UPDATE activos SET Estado = 'Activo' WHERE Estado = 'Operativo'");
            migrationBuilder.Sql("UPDATE activos SET Estado = 'EnMantenimiento' WHERE Estado = 'Mantenimiento'");
            migrationBuilder.Sql("UPDATE activos SET Estado = 'Baja' WHERE Estado = 'Inactivo'");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropColumn(
                name: "EficienciaPct",
                table: "activos");

            migrationBuilder.DropColumn(
                name: "FinGarantia",
                table: "activos");

            migrationBuilder.DropColumn(
                name: "PotenciaNominalKw",
                table: "activos");

            migrationBuilder.DropColumn(
                name: "ProximaRevision",
                table: "activos");

            migrationBuilder.DropColumn(
                name: "UltimaRevision",
                table: "activos");
        }
    }
}
