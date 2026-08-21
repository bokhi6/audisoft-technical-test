using AudiSoft.Application.Estudiantes;
using AudiSoft.Application.Notas;
using AudiSoft.Domain.Entities;
using AudiSoft.Domain.Exceptions;
using Moq;

namespace AudiSoft.Application.Tests;

public class EstudianteServiceTests
{
    private readonly Mock<IEstudianteRepository> _estudianteRepository = new();
    private readonly Mock<INotaRepository> _notaRepository = new();
    private readonly EstudianteService _sut;

    public EstudianteServiceTests()
    {
        _sut = new EstudianteService(_estudianteRepository.Object, _notaRepository.Object);
    }

    [Fact]
    public async Task CrearAsync_ConNombreVacio_LanzaValidationAppException()
    {
        var dto = new CrearEstudianteDto("   ");

        await Assert.ThrowsAsync<ValidationAppException>(() => _sut.CrearAsync(dto));

        _estudianteRepository.Verify(r => r.AgregarAsync(It.IsAny<Estudiante>()), Times.Never);
    }

    [Fact]
    public async Task CrearAsync_ConNombreValido_CreaYRetornaDto()
    {
        var dto = new CrearEstudianteDto("Juan Pérez");

        var resultado = await _sut.CrearAsync(dto);

        Assert.Equal("Juan Pérez", resultado.Nombre);
        _estudianteRepository.Verify(r => r.AgregarAsync(It.Is<Estudiante>(e => e.Nombre == "Juan Pérez")), Times.Once);
        _estudianteRepository.Verify(r => r.GuardarCambiosAsync(), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_ConNotasAsociadas_LanzaConflictoDeIntegridadException()
    {
        var estudiante = new Estudiante { Id = 1, Nombre = "Juan Pérez" };
        _estudianteRepository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(estudiante);
        _notaRepository.Setup(r => r.ExisteAsociadaAEstudianteAsync(1)).ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictoDeIntegridadException>(() => _sut.EliminarAsync(1));

        _estudianteRepository.Verify(r => r.Eliminar(It.IsAny<Estudiante>()), Times.Never);
    }

    [Fact]
    public async Task EliminarAsync_SinNotasAsociadas_EliminaCorrectamente()
    {
        var estudiante = new Estudiante { Id = 2, Nombre = "María Gómez" };
        _estudianteRepository.Setup(r => r.ObtenerPorIdAsync(2)).ReturnsAsync(estudiante);
        _notaRepository.Setup(r => r.ExisteAsociadaAEstudianteAsync(2)).ReturnsAsync(false);

        await _sut.EliminarAsync(2);

        _estudianteRepository.Verify(r => r.Eliminar(estudiante), Times.Once);
        _estudianteRepository.Verify(r => r.GuardarCambiosAsync(), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_EstudianteInexistente_LanzaNotFoundException()
    {
        _estudianteRepository.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Estudiante?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.EliminarAsync(99));
    }
}
