using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Challenge_Clyvo_Vet_DotNet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_CLV_PET",
                columns: table => new
                {
                    ID_PET = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_RESPONSAVEL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOME_PET = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ESPECIE_PET = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    RACA_PET = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DATA_NASCIMENTO_PET = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    STATUS_CASTRADO = table.Column<string>(type: "NCHAR(1)", fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLV_PET", x => x.ID_PET);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_CLV_PET");
        }
    }
}
