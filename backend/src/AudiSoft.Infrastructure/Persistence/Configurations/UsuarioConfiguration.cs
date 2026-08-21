using AudiSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudiSoft.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.NombreUsuario)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.NombreUsuario).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(200);

        // Usuario administrador de ejemplo. Contraseña: Audisoft2026!
        // (hash PBKDF2 generado con AudiSoft.Domain.Security.PasswordHasher)
        builder.HasData(
            new Usuario
            {
                Id = 1,
                NombreUsuario = "admin",
                PasswordHash = "100000.1JvNEpZMYCX38xYzYqLkyw==.HuUv4EiH+OrkB0eNlHkAM4ewcL7f/IQsJvDC3LHMeSQ="
            }
        );
    }
}
