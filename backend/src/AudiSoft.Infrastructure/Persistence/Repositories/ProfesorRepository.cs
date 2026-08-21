using AudiSoft.Application.Profesores;
using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudiSoft.Infrastructure.Persistence.Repositories;

public class ProfesorRepository : IProfesorRepository
{
    private readonly AudiSoftDbContext _context;

    public ProfesorRepository(AudiSoftDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Profesor> Items, int TotalCount)> ObtenerPaginadoAsync(int pageNumber, int pageSize)
    {
        var query = _context.Profesores.AsNoTracking().OrderBy(p => p.Id);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<Profesor>> ObtenerTodosAsync()
        => await _context.Profesores.AsNoTracking().OrderBy(p => p.Nombre).ToListAsync();

    public async Task<Profesor?> ObtenerPorIdAsync(int id)
        => await _context.Profesores.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<bool> ExisteAsync(int id)
        => await _context.Profesores.AnyAsync(p => p.Id == id);

    public async Task AgregarAsync(Profesor profesor)
        => await _context.Profesores.AddAsync(profesor);

    public void Actualizar(Profesor profesor)
        => _context.Profesores.Update(profesor);

    public void Eliminar(Profesor profesor)
        => _context.Profesores.Remove(profesor);

    public async Task GuardarCambiosAsync()
        => await _context.SaveChangesAsync();
}
