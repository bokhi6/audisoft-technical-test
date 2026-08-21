namespace AudiSoft.Application.Auth;

public record LoginDto(string NombreUsuario, string Password);

public record TokenResponseDto(string Token, string NombreUsuario, DateTime ExpiraEn);
