using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IConnet.Presale.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRootCause : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "classification",
                table: "dbo.root_cause",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "is_on_verification",
                table: "dbo.root_cause",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "classification",
                table: "dbo.root_cause");

            migrationBuilder.DropColumn(
                name: "is_on_verification",
                table: "dbo.root_cause");
        }
    }
}
