using AudiSoft.Domain.Entities;

namespace AudiSoft.Application.Auth;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
}
