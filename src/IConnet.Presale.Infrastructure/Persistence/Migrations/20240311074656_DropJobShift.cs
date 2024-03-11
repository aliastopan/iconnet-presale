using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IConnet.Presale.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropJobShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "job_shift",
                table: "dbo.user_account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "job_shift",
                table: "dbo.user_account",
                type: "int",
                maxLength: 32,
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 6);
        }
    }
}
