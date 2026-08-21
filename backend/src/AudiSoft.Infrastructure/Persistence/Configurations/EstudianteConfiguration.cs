using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudiSoft.Infrastructure.Persistence.Configurations;

public class EstudianteConfiguration : IEntityTypeConfiguration<Estudiante>
{
    public void Configure(EntityTypeBuilder<Estudiante> builder)
    {
        builder.ToTable("Estudiantes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasData(
            new Estudiante { Id = 1, Nombre = "Juan Pérez" },
            new Estudiante { Id = 2, Nombre = "María Gómez" },
            new Estudiante { Id = 3, Nombre = "Carlos Rodríguez" },
            new Estudiante { Id = 4, Nombre = "Ana Martínez" },
            new Estudiante { Id = 5, Nombre = "Luis Fernández" }
        );
    }
}
