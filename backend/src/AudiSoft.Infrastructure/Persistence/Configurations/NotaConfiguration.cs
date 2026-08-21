using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudiSoft.Infrastructure.Persistence.Configurations;

public class NotaConfiguration : IEntityTypeConfiguration<Nota>
{
    public void Configure(EntityTypeBuilder<Nota> builder)
    {
        builder.ToTable("Notas");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Valor)
            .HasColumnType("decimal(5,2)");

        // Restrict evita cascada de borrado: si un Estudiante o Profesor tiene
        // notas asociadas, SQL Server rechaza el DELETE en vez de borrar en cadena.
        // La capa Application valida esto antes y traduce el conflicto a un 409 amigable.
        builder.HasOne(n => n.Estudiante)
            .WithMany(e => e.Notas)
            .HasForeignKey(n => n.IdEstudiante)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.Profesor)
            .WithMany(p => p.Notas)
            .HasForeignKey(n => n.IdProfesor)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new { Id = 1, Nombre = "Parcial 1", IdEstudiante = 1, IdProfesor = 1, Valor = 3.5m },
            new { Id = 2, Nombre = "Parcial 2", IdEstudiante = 2, IdProfesor = 2, Valor = 4.2m },
            new { Id = 3, Nombre = "Quiz 1", IdEstudiante = 3, IdProfesor = 3, Valor = 2.8m },
            new { Id = 4, Nombre = "Proyecto Final", IdEstudiante = 4, IdProfesor = 1, Valor = 4.9m },
            new { Id = 5, Nombre = "Examen Final", IdEstudiante = 5, IdProfesor = 2, Valor = 3.0m }
        );
    }
}
