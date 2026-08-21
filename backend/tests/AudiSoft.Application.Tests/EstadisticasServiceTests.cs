using AudiSoft.Application.Estadisticas;
using AudiSoft.Application.Estudiantes;
using AudiSoft.Application.Notas;
using AudiSoft.Domain.Entities;
using Moq;

namespace AudiSoft.Application.Tests;

public class EstadisticasServiceTests
{
    private readonly Mock<INotaRepository> _notaRepository = new();
    private readonly Mock<IEstudianteRepository> _estudianteRepository = new();
    private readonly EstadisticasService _sut;

    public EstadisticasServiceTests()
    {
        _sut = new EstadisticasService(_notaRepository.Object, _estudianteRepository.Object);
    }

    [Fact]
    public async Task ObtenerResumenAsync_ConDatosDelSeed_CalculaCorrectamente()
    {
        var notas = new List<Nota>
        {
            new() { Id = 1, IdEstudiante = 1, IdProfesor = 1, Valor = 3.5m },
            new() { Id = 2, IdEstudiante = 2, IdProfesor = 2, Valor = 4.2m },
            new() { Id = 3, IdEstudiante = 3, IdProfesor = 3, Valor = 2.8m },
            new() { Id = 4, IdEstudiante = 4, IdProfesor = 1, Valor = 4.9m },
            new() { Id = 5, IdEstudiante = 5, IdProfesor = 2, Valor = 3.0m }
        };
        var estudiantes = Enumerable.Range(1, 5).Select(id => new Estudiante { Id = id, Nombre = $"Estudiante {id}" }).ToList();

        _notaRepository.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(notas);
        _estudianteRepository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(estudiantes);

        var resultado = await _sut.ObtenerResumenAsync();

        Assert.Equal(3.68m, resultado.PromedioGeneral);
        Assert.Equal(5, resultado.TotalNotas);
        Assert.Equal(4, resultado.NotasAprobadas);
        Assert.Equal(1, resultado.NotasReprobadas);
        Assert.Equal(5, resultado.TotalEstudiantes);
        Assert.Equal(4, resultado.EstudiantesAprobados);
        Assert.Equal(1, resultado.EstudiantesReprobados);
        Assert.Equal(0, resultado.EstudiantesSinNotas);
        Assert.Equal(80.0m, resultado.PorcentajeEstudiantesAprobados);
        Assert.Equal(20.0m, resultado.PorcentajeEstudiantesReprobados);
    }

    [Fact]
    public async Task ObtenerResumenAsync_SinNotas_NoLanzaYDevuelveCeros()
    {
        _notaRepository.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(new List<Nota>());
        _estudianteRepository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<Estudiante> { new() { Id = 1, Nombre = "Solo" } });

        var resultado = await _sut.ObtenerResumenAsync();

        Assert.Equal(0m, resultado.PromedioGeneral);
        Assert.Equal(0, resultado.TotalNotas);
        Assert.Equal(1, resultado.EstudiantesSinNotas);
    }
}
