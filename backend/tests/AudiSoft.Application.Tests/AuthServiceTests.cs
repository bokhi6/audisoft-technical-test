using AudiSoft.Application.Auth;
using AudiSoft.Domain.Entities;
using AudiSoft.Domain.Exceptions;
using AudiSoft.Domain.Security;
using Moq;

namespace AudiSoft.Application.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepository = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_usuarioRepository.Object, _jwtTokenGenerator.Object);
    }

    [Fact]
    public async Task IniciarSesionAsync_ConCredencialesValidas_RetornaToken()
    {
        var usuario = new Usuario { Id = 1, NombreUsuario = "admin", PasswordHash = PasswordHasher.Hash("Clave123!") };
        _usuarioRepository.Setup(r => r.ObtenerPorNombreUsuarioAsync("admin")).ReturnsAsync(usuario);
        _jwtTokenGenerator.Setup(g => g.GenerarToken(1, "admin")).Returns(("token-generado", DateTime.UtcNow.AddHours(1)));

        var resultado = await _sut.IniciarSesionAsync(new LoginDto("admin", "Clave123!"));

        Assert.Equal("token-generado", resultado.Token);
        Assert.Equal("admin", resultado.NombreUsuario);
    }

    [Fact]
    public async Task IniciarSesionAsync_ConPasswordIncorrecta_LanzaAutenticacionException()
    {
        var usuario = new Usuario { Id = 1, NombreUsuario = "admin", PasswordHash = PasswordHasher.Hash("Clave123!") };
        _usuarioRepository.Setup(r => r.ObtenerPorNombreUsuarioAsync("admin")).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<AutenticacionException>(
            () => _sut.IniciarSesionAsync(new LoginDto("admin", "incorrecta")));

        _jwtTokenGenerator.Verify(g => g.GenerarToken(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task IniciarSesionAsync_ConUsuarioInexistente_LanzaAutenticacionException()
    {
        _usuarioRepository.Setup(r => r.ObtenerPorNombreUsuarioAsync("noexiste")).ReturnsAsync((Usuario?)null);

        await Assert.ThrowsAsync<AutenticacionException>(
            () => _sut.IniciarSesionAsync(new LoginDto("noexiste", "cualquiera")));
    }
}
