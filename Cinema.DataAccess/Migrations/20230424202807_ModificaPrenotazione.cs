using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModificaPrenotazione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Pagato",
                table: "Prenotazione",
                type: "boolean(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean(1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Pagato",
                table: "Prenotazione",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldNullable: true);
        }
    }
}
