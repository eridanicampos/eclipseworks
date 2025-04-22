using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTest.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nome",
                table: "historico_alteracao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "historico_alteracao",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
