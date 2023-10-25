using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDAL.IndexerMigrations
{
    /// <inheritdoc />
    public partial class IndexedWordPostCountColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostsCount",
                table: "IndexedWords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostsCount",
                table: "IndexedWords");
        }
    }
}
