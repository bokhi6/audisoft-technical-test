using AudiSoft.Application.Notas;
using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudiSoft.Infrastructure.Persistence.Repositories;

public class NotaRepository : INotaRepository
{
    private readonly AudiSoftDbContext _context;

    public NotaRepository(AudiSoftDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Nota> Items, int TotalCount)> ObtenerPaginadoAsync(int pageNumber, int pageSize)
    {
        var query = _context.Notas
            .AsNoTracking()
            .Include(n => n.Estudiante)
            .Include(n => n.Profesor)
            .OrderBy(n => n.Id);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Nota?> ObtenerPorIdAsync(int id)
        => await _context.Notas
            .Include(n => n.Estudiante)
            .Include(n => n.Profesor)
            .FirstOrDefaultAsync(n => n.Id == id);

    public async Task<bool> ExisteAsociadaAEstudianteAsync(int idEstudiante)
        => await _context.Notas.AnyAsync(n => n.IdEstudiante == idEstudiante);

    public async Task<bool> ExisteAsociadaAProfesorAsync(int idProfesor)
        => await _context.Notas.AnyAsync(n => n.IdProfesor == idProfesor);

    public async Task AgregarAsync(Nota nota)
        => await _context.Notas.AddAsync(nota);

    public void Actualizar(Nota nota)
        => _context.Notas.Update(nota);

    public void Eliminar(Nota nota)
        => _context.Notas.Remove(nota);

    public async Task GuardarCambiosAsync()
        => await _context.SaveChangesAsync();
}
