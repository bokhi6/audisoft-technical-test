using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudiSoft.Infrastructure.Persistence.Configurations;

public class ProfesorConfiguration : IEntityTypeConfiguration<Profesor>
{
    public void Configure(EntityTypeBuilder<Profesor> builder)
    {
        builder.ToTable("Profesores");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasData(
            new Profesor { Id = 1, Nombre = "Andrés Torres" },
            new Profesor { Id = 2, Nombre = "Beatriz Ramírez" },
            new Profesor { Id = 3, Nombre = "Camilo Vargas" },
            new Profesor { Id = 4, Nombre = "Diana Castro" },
            new Profesor { Id = 5, Nombre = "Eduardo Salazar" }
        );
    }
}
