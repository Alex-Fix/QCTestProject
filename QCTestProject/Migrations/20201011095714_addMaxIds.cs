using Microsoft.EntityFrameworkCore.Migrations;

namespace QCTestProject.Migrations
{
    public partial class addMaxIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaxIds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxBookId = table.Column<int>(nullable: false),
                    MaxAuthorId = table.Column<int>(nullable: false),
                    MaxCategoryId = table.Column<int>(nullable: false),
                    MaxLanguageId = table.Column<int>(nullable: false),
                    MaxPublisherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaxIds", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaxIds");
        }
    }
}
