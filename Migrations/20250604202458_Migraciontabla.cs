using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAerolineaWeb.Migrations
{
    /// <inheritdoc />
    public partial class Migraciontabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasajerosId",
                table: "DetallesPasajero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PasajerosId",
                table: "DetallesPasajero",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
