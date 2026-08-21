using AudiSoft.Application.Estudiantes;
using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudiSoft.Infrastructure.Persistence.Repositories;

public class EstudianteRepository : IEstudianteRepository
{
    private readonly AudiSoftDbContext _context;

    public EstudianteRepository(AudiSoftDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Estudiante> Items, int TotalCount)> ObtenerPaginadoAsync(int pageNumber, int pageSize)
    {
        var query = _context.Estudiantes.AsNoTracking().OrderBy(e => e.Id);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<Estudiante>> ObtenerTodosAsync()
        => await _context.Estudiantes.AsNoTracking().OrderBy(e => e.Nombre).ToListAsync();

    public async Task<Estudiante?> ObtenerPorIdAsync(int id)
        => await _context.Estudiantes.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<bool> ExisteAsync(int id)
        => await _context.Estudiantes.AnyAsync(e => e.Id == id);

    public async Task AgregarAsync(Estudiante estudiante)
        => await _context.Estudiantes.AddAsync(estudiante);

    public void Actualizar(Estudiante estudiante)
        => _context.Estudiantes.Update(estudiante);

    public void Eliminar(Estudiante estudiante)
        => _context.Estudiantes.Remove(estudiante);

    public async Task GuardarCambiosAsync()
        => await _context.SaveChangesAsync();
}
