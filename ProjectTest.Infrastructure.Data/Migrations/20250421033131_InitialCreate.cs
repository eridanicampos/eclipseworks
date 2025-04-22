using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTest.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    role = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "acesso_usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acesso_usuario", x => x.id);
                    table.ForeignKey(
                        name: "FK_acesso_usuario_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projeto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projeto", x => x.id);
                    table.ForeignKey(
                        name: "FK_projeto_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tarefa",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    data_vencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    prioridade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    projeto_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tarefa", x => x.id);
                    table.ForeignKey(
                        name: "FK_tarefa_projeto_projeto_id",
                        column: x => x.projeto_id,
                        principalTable: "projeto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tarefa_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comentario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    texto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tarefa_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comentario", x => x.id);
                    table.ForeignKey(
                        name: "FK_comentario_tarefa_tarefa_id",
                        column: x => x.tarefa_id,
                        principalTable: "tarefa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comentario_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "historico_alteracao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tarefa_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    json_tarefa_antes_alterada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    json_tarefa_depois_alterada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    campos_alterados = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    data_alteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_alteracao", x => x.id);
                    table.ForeignKey(
                        name: "FK_historico_alteracao_tarefa_tarefa_id",
                        column: x => x.tarefa_id,
                        principalTable: "tarefa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_historico_alteracao_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_acesso_usuario_usuario_id",
                table: "acesso_usuario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_comentario_tarefa_id",
                table: "comentario",
                column: "tarefa_id");

            migrationBuilder.CreateIndex(
                name: "IX_comentario_usuario_id",
                table: "comentario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_historico_alteracao_tarefa_id",
                table: "historico_alteracao",
                column: "tarefa_id");

            migrationBuilder.CreateIndex(
                name: "IX_historico_alteracao_usuario_id",
                table: "historico_alteracao",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_projeto_usuario_id",
                table: "projeto",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tarefa_projeto_id",
                table: "tarefa",
                column: "projeto_id");

            migrationBuilder.CreateIndex(
                name: "IX_tarefa_usuario_id",
                table: "tarefa",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_email",
                table: "usuario",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "acesso_usuario");

            migrationBuilder.DropTable(
                name: "comentario");

            migrationBuilder.DropTable(
                name: "historico_alteracao");

            migrationBuilder.DropTable(
                name: "tarefa");

            migrationBuilder.DropTable(
                name: "projeto");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
