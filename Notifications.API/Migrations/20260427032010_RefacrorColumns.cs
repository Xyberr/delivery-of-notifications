using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notifications.API.Migrations
{
    /// <inheritdoc />
    public partial class RefacrorColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "ApiKeys",
                newName: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ApiKeys",
                newName: "CreateBy");
        }
    }
}
