namespace AudiSoft.Application.Auth;

public interface IAuthService
{
    Task<TokenResponseDto> IniciarSesionAsync(LoginDto dto);
}
