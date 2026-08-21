using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudiSoft.Infrastructure.Persistence;

public class AudiSoftDbContext : DbContext
{
    public AudiSoftDbContext(DbContextOptions<AudiSoftDbContext> options) : base(options) { }

    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Profesor> Profesores => Set<Profesor>();
    public DbSet<Nota> Notas => Set<Nota>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AudiSoftDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
