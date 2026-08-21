namespace AudiSoft.Application.Auth;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiraEn) GenerarToken(int usuarioId, string nombreUsuario);
}
