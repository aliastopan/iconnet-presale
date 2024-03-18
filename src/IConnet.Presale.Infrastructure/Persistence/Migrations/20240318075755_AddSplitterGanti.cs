using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IConnet.Presale.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSplitterGanti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "approval_splitter_ganti",
                table: "dbo.work_paper",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "approval_splitter_ganti",
                table: "dbo.work_paper");
        }
    }
}
