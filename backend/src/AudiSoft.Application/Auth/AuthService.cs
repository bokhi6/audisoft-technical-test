using AudiSoft.Domain.Exceptions;
using AudiSoft.Domain.Security;

namespace AudiSoft.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUsuarioRepository usuarioRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _usuarioRepository = usuarioRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<TokenResponseDto> IniciarSesionAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NombreUsuario) || string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new AutenticacionException("Usuario o contraseña incorrectos.");
        }

        var usuario = await _usuarioRepository.ObtenerPorNombreUsuarioAsync(dto.NombreUsuario);
        if (usuario is null || !PasswordHasher.Verificar(dto.Password, usuario.PasswordHash))
        {
            throw new AutenticacionException("Usuario o contraseña incorrectos.");
        }

        var (token, expiraEn) = _jwtTokenGenerator.GenerarToken(usuario.Id, usuario.NombreUsuario);
        return new TokenResponseDto(token, usuario.NombreUsuario, expiraEn);
    }
}
