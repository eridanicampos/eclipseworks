using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("nome");


            builder.Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("senha");


            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("email");

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("role");

            builder.HasIndex(u => u.Email) 
               .IsUnique();

            builder.HasMany(u => u.Projetos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AcessosUsuarios)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            new EntityGuidMap<Usuario>().AddCommonConfiguration(builder);
        }
    }
}
