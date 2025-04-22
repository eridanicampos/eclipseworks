using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class ComentarioMap : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.ToTable("comentario");

            builder.Property(c => c.Texto)
                .HasColumnName("texto")
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(h => h.UsuarioId)
                .HasColumnName("usuario_id");

            builder.Property(h => h.TarefaId)
                .IsRequired()
                .HasColumnName("tarefa_id");

            builder.HasOne(c => c.Usuario)
                .WithMany() 
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Tarefa)
                .WithMany(t => t.Comentarios)
                .HasForeignKey(c => c.TarefaId)
                .OnDelete(DeleteBehavior.Cascade);

            new EntityGuidMap<Comentario>().AddCommonConfiguration(builder);
        }
    }
}
