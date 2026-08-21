using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AudiSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InicialConSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdEstudiante = table.Column<int>(type: "int", nullable: false),
                    IdProfesor = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notas_Estudiantes_IdEstudiante",
                        column: x => x.IdEstudiante,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notas_Profesores_IdProfesor",
                        column: x => x.IdProfesor,
                        principalTable: "Profesores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Estudiantes",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Juan Pérez" },
                    { 2, "María Gómez" },
                    { 3, "Carlos Rodríguez" },
                    { 4, "Ana Martínez" },
                    { 5, "Luis Fernández" }
                });

            migrationBuilder.InsertData(
                table: "Profesores",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Andrés Torres" },
                    { 2, "Beatriz Ramírez" },
                    { 3, "Camilo Vargas" },
                    { 4, "Diana Castro" },
                    { 5, "Eduardo Salazar" }
                });

            migrationBuilder.InsertData(
                table: "Notas",
                columns: new[] { "Id", "IdEstudiante", "IdProfesor", "Nombre", "Valor" },
                values: new object[,]
                {
                    { 1, 1, 1, "Parcial 1", 3.5m },
                    { 2, 2, 2, "Parcial 2", 4.2m },
                    { 3, 3, 3, "Quiz 1", 2.8m },
                    { 4, 4, 1, "Proyecto Final", 4.9m },
                    { 5, 5, 2, "Examen Final", 3.0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_IdEstudiante",
                table: "Notas",
                column: "IdEstudiante");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_IdProfesor",
                table: "Notas",
                column: "IdProfesor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Profesores");
        }
    }
}
