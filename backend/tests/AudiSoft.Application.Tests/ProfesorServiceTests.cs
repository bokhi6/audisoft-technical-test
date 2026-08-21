using AudiSoft.Application.Notas;
using AudiSoft.Application.Profesores;
using AudiSoft.Domain.Entities;
using AudiSoft.Domain.Exceptions;
using Moq;

namespace AudiSoft.Application.Tests;

public class ProfesorServiceTests
{
    private readonly Mock<IProfesorRepository> _profesorRepository = new();
    private readonly Mock<INotaRepository> _notaRepository = new();
    private readonly ProfesorService _sut;

    public ProfesorServiceTests()
    {
        _sut = new ProfesorService(_profesorRepository.Object, _notaRepository.Object);
    }

    [Fact]
    public async Task CrearAsync_ConNombreVacio_LanzaValidationAppException()
    {
        var dto = new CrearProfesorDto("");

        await Assert.ThrowsAsync<ValidationAppException>(() => _sut.CrearAsync(dto));

        _profesorRepository.Verify(r => r.AgregarAsync(It.IsAny<Profesor>()), Times.Never);
    }

    [Fact]
    public async Task EliminarAsync_ConNotasAsociadas_LanzaConflictoDeIntegridadException()
    {
        var profesor = new Profesor { Id = 1, Nombre = "Andrés Torres" };
        _profesorRepository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(profesor);
        _notaRepository.Setup(r => r.ExisteAsociadaAProfesorAsync(1)).ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictoDeIntegridadException>(() => _sut.EliminarAsync(1));

        _profesorRepository.Verify(r => r.Eliminar(It.IsAny<Profesor>()), Times.Never);
    }

    [Fact]
    public async Task EliminarAsync_SinNotasAsociadas_EliminaCorrectamente()
    {
        var profesor = new Profesor { Id = 4, Nombre = "Diana Castro" };
        _profesorRepository.Setup(r => r.ObtenerPorIdAsync(4)).ReturnsAsync(profesor);
        _notaRepository.Setup(r => r.ExisteAsociadaAProfesorAsync(4)).ReturnsAsync(false);

        await _sut.EliminarAsync(4);

        _profesorRepository.Verify(r => r.Eliminar(profesor), Times.Once);
        _profesorRepository.Verify(r => r.GuardarCambiosAsync(), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_ProfesorInexistente_LanzaNotFoundException()
    {
        _profesorRepository.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Profesor?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.EliminarAsync(99));
    }
}
