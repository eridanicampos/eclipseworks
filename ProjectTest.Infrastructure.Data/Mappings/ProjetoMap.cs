using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class ProjetoMap : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> builder)
        {
            builder.ToTable("projeto");

            builder.Property(p => p.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(h => h.UsuarioId)
                .HasColumnName("usuario_id");

            builder.HasOne(p => p.Usuario)
                .WithMany(u => u.Projetos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Tarefas)
                .WithOne(t => t.Projeto)
                .HasForeignKey(t => t.ProjetoId)
                .OnDelete(DeleteBehavior.Cascade);

            new EntityGuidMap<Projeto>().AddCommonConfiguration(builder);
        }
    }
}
