using AudiSoft.Application.Estudiantes;
using AudiSoft.Application.Notas;
using AudiSoft.Application.Profesores;
using AudiSoft.Domain.Entities;
using AudiSoft.Domain.Exceptions;
using Moq;

namespace AudiSoft.Application.Tests;

public class NotaServiceTests
{
    private readonly Mock<INotaRepository> _notaRepository = new();
    private readonly Mock<IEstudianteRepository> _estudianteRepository = new();
    private readonly Mock<IProfesorRepository> _profesorRepository = new();
    private readonly NotaService _sut;

    public NotaServiceTests()
    {
        _sut = new NotaService(_notaRepository.Object, _estudianteRepository.Object, _profesorRepository.Object);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5.1)]
    public async Task CrearAsync_ValorFueraDeRango_LanzaValidationAppException(decimal valor)
    {
        _estudianteRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);
        _profesorRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);

        var dto = new CrearNotaDto("Parcial 1", 1, 1, valor);

        await Assert.ThrowsAsync<ValidationAppException>(() => _sut.CrearAsync(dto));

        _notaRepository.Verify(r => r.AgregarAsync(It.IsAny<Nota>()), Times.Never);
    }

    [Fact]
    public async Task CrearAsync_EstudianteInexistente_LanzaNotFoundException()
    {
        _estudianteRepository.Setup(r => r.ExisteAsync(99)).ReturnsAsync(false);
        _profesorRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);

        var dto = new CrearNotaDto("Parcial 1", 99, 1, 4.0m);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.CrearAsync(dto));
    }

    [Fact]
    public async Task CrearAsync_ProfesorInexistente_LanzaNotFoundException()
    {
        _estudianteRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);
        _profesorRepository.Setup(r => r.ExisteAsync(99)).ReturnsAsync(false);

        var dto = new CrearNotaDto("Parcial 1", 1, 99, 4.0m);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.CrearAsync(dto));
    }

    [Fact]
    public async Task CrearAsync_DatosValidos_CreaCorrectamente()
    {
        _estudianteRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);
        _profesorRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);

        // AgregarAsync no asigna Id real (no hay base de datos detrás del mock),
        // por lo que la nota recien creada conserva Id = 0 hasta el re-fetch.
        var notaCreada = new Nota
        {
            Id = 0,
            Nombre = "Parcial 1",
            IdEstudiante = 1,
            IdProfesor = 1,
            Valor = 4.0m,
            Estudiante = new Estudiante { Id = 1, Nombre = "Juan Pérez" },
            Profesor = new Profesor { Id = 1, Nombre = "Andrés Torres" }
        };
        _notaRepository.Setup(r => r.ObtenerPorIdAsync(0)).ReturnsAsync(notaCreada);

        var dto = new CrearNotaDto("Parcial 1", 1, 1, 4.0m);

        var resultado = await _sut.CrearAsync(dto);

        Assert.Equal("Juan Pérez", resultado.NombreEstudiante);
        Assert.Equal("Andrés Torres", resultado.NombreProfesor);
        Assert.Equal(4.0m, resultado.Valor);
        _notaRepository.Verify(r => r.AgregarAsync(It.Is<Nota>(n => n.Nombre == "Parcial 1")), Times.Once);
        _notaRepository.Verify(r => r.GuardarCambiosAsync(), Times.Once);
    }
}
