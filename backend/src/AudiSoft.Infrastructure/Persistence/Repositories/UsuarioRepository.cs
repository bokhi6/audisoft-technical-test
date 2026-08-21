using AudiSoft.Application.Auth;
using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudiSoft.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AudiSoftDbContext _context;

    public UsuarioRepository(AudiSoftDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
        => await _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
}
