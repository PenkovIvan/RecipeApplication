using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeApplication.Migrations
{
    public partial class EtraFieldCreateById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateById",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateById",
                table: "Recipes");
        }
    }
}
