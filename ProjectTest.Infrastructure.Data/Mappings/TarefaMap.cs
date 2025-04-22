using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class TarefaMap : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.ToTable("tarefa");

            builder.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("titulo");

            builder.Property(t => t.Descricao)
                .HasMaxLength(1000)
                .HasColumnName("descricao");

            builder.Property(t => t.DataVencimento)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasColumnName("data_vencimento");

            builder.Property(t => t.Status)
                .HasConversion<string>(
                    c => c.ToString(),
                    c => (EStatusTarefa)Enum.Parse(typeof(EStatusTarefa), c))
                .HasColumnName("status")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Prioridade)
                .HasConversion<string>(
                    c => c.ToString(),
                    c => (EPrioridade)Enum.Parse(typeof(EPrioridade), c))
                .HasColumnName("prioridade")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.ProjetoId)
                .HasColumnName("projeto_id");

            builder.HasOne(t => t.Projeto)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(t => t.ProjetoId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Property(t => t.UsuarioId)
                .HasColumnName("usuario_id");

            builder.HasOne(t => t.Usuario)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(t => t.Historico)
                .WithOne(h => h.Tarefa)
                .HasForeignKey(h => h.TarefaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Comentarios)
                .WithOne(c => c.Tarefa)
                .HasForeignKey(c => c.TarefaId)
                .OnDelete(DeleteBehavior.Restrict);

            new EntityGuidMap<Tarefa>().AddCommonConfiguration(builder);
        }
    }
}
