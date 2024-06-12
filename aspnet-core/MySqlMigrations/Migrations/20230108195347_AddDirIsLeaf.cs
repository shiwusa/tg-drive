using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgDrive.DataAccess.MySqlMigrations
{
    public partial class AddDirIsLeaf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Leaf",
                table: "Directories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leaf",
                table: "Directories");
        }
    }
}
