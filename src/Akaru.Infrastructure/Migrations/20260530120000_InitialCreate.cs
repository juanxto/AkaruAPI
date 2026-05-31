using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akaru.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "AKARU");

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                schema: "AKARU",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FIREBASE_UID = table.Column<string>(type: "VARCHAR2(128)", maxLength: 128, nullable: false),
                    NOME = table.Column<string>(type: "VARCHAR2(150)", maxLength: 150, nullable: false),
                    EMAIL = table.Column<string>(type: "VARCHAR2(200)", maxLength: 200, nullable: false),
                    LATITUDE = table.Column<decimal>(type: "NUMBER(10,7)", precision: 10, scale: 7, nullable: true),
                    LONGITUDE = table.Column<decimal>(type: "NUMBER(10,7)", precision: 10, scale: 7, nullable: true),
                    CIDADE = table.Column<string>(type: "VARCHAR2(100)", maxLength: 100, nullable: true),
                    ESTADO = table.Column<string>(type: "VARCHAR2(2)", maxLength: 2, nullable: true),
                    DT_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_PLANTIO",
                schema: "AKARU",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_CULTURA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LATITUDE = table.Column<decimal>(type: "NUMBER(10,7)", precision: 10, scale: 7, nullable: false),
                    LONGITUDE = table.Column<decimal>(type: "NUMBER(10,7)", precision: 10, scale: 7, nullable: false),
                    CIDADE = table.Column<string>(type: "VARCHAR2(100)", maxLength: 100, nullable: true),
                    ESTADO = table.Column<string>(type: "VARCHAR2(2)", maxLength: 2, nullable: true),
                    DT_PLANTIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DETALHES = table.Column<string>(type: "VARCHAR2(2000)", maxLength: 2000, nullable: true),
                    DT_REGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PLANTIO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_PLANTIO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalSchema: "AKARU",
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_HISTORICO_RECOMENDACAO",
                schema: "AKARU",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_CULTURA = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOME_CULTURA = table.Column<string>(type: "VARCHAR2(150)", maxLength: 150, nullable: true),
                    TEXTO = table.Column<string>(type: "CLOB", nullable: false),
                    SCORE = table.Column<decimal>(type: "NUMBER(5,2)", precision: 5, scale: 2, nullable: true),
                    LATITUDE = table.Column<decimal>(type: "NUMBER(10,7)", precision: 10, scale: 7, nullable: true),
                    LONGITUDE = table.Column<decimal>(type: "NUMBER(10,7)", precision: 10, scale: 7, nullable: true),
                    DETALHES = table.Column<string>(type: "VARCHAR2(2000)", maxLength: 2000, nullable: true),
                    DADOS_CLIMA = table.Column<string>(type: "VARCHAR2(4000)", maxLength: 4000, nullable: true),
                    DT_GERACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_HISTORICO_RECOMENDACAO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_HISTORICO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalSchema: "AKARU",
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_PLANTIO_CULTURA",
                schema: "AKARU",
                columns: table => new
                {
                    ID_PLANTIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_CULTURA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PLANTIO_CULTURA", x => new { x.ID_PLANTIO, x.ID_CULTURA });
                    table.ForeignKey(
                        name: "FK_TB_PC_TB_PLANTIO_ID_PLANTIO",
                        column: x => x.ID_PLANTIO,
                        principalSchema: "AKARU",
                        principalTable: "TB_PLANTIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_FIREBASE_UID",
                schema: "AKARU",
                table: "TB_USUARIO",
                column: "FIREBASE_UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_EMAIL",
                schema: "AKARU",
                table: "TB_USUARIO",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_PLANTIO_ID_USUARIO",
                schema: "AKARU",
                table: "TB_PLANTIO",
                column: "ID_USUARIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TB_PLANTIO_CULTURA", schema: "AKARU");
            migrationBuilder.DropTable(name: "TB_HISTORICO_RECOMENDACAO", schema: "AKARU");
            migrationBuilder.DropTable(name: "TB_PLANTIO", schema: "AKARU");
            migrationBuilder.DropTable(name: "TB_USUARIO", schema: "AKARU");
        }
    }
}
