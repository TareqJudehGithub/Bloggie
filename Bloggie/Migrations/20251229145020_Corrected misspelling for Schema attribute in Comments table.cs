using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedmisspellingforSchemaattributeinCommentstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "dob",
                newName: "Comments",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dob");

            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "dbo",
                newName: "Comments",
                newSchema: "dob");
        }
    }
}
