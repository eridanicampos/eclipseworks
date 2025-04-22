using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class HistoricoAlteracaoMap : IEntityTypeConfiguration<HistoricoAlteracao>
    {
        public void Configure(EntityTypeBuilder<HistoricoAlteracao> builder)
        {
            builder.ToTable("historico_alteracao");

            builder.Property(h => h.Tipo)
                .IsRequired()
                .HasConversion<string>(
                    v => v.ToString(),
                    v => (ETipoAllteracao)Enum.Parse(typeof(ETipoAllteracao), v))
                .HasColumnName("tipo")
                .HasMaxLength(100);

            builder.Property(h => h.JsonTarefaAntesAlterada)
                .HasColumnType("nvarchar(max)")
                .HasColumnName("json_tarefa_antes_alterada");

            builder.Property(h => h.JsonTarefaDepoisAlterada)
                .HasColumnType("nvarchar(max)")
                .HasColumnName("json_tarefa_depois_alterada");

            builder.Property(h => h.CamposAlterados)
                .HasMaxLength(500)
                .HasColumnName("campos_alterados");

            builder.Property(h => h.DataAlteracao)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasColumnName("data_alteracao");

            builder.Property(h => h.UsuarioId)
                .HasColumnName("usuario_id");

            builder.Property(h => h.TarefaId)
                .HasColumnName("tarefa_id");

            builder.HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Tarefa)
                .WithMany(t => t.Historico)
                .HasForeignKey(h => h.TarefaId)
                .OnDelete(DeleteBehavior.Cascade);

            new EntityGuidMap<HistoricoAlteracao>().AddCommonConfiguration(builder);
        }
    }
}
