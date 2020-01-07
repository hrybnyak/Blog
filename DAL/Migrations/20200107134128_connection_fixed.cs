using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class connection_fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTegs_Articles_ArticleId",
                table: "ArticleTegs");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTegs_Tegs_TegId",
                table: "ArticleTegs");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTegs_Articles_ArticleId",
                table: "ArticleTegs",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTegs_Tegs_TegId",
                table: "ArticleTegs",
                column: "TegId",
                principalTable: "Tegs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTegs_Articles_ArticleId",
                table: "ArticleTegs");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTegs_Tegs_TegId",
                table: "ArticleTegs");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTegs_Articles_ArticleId",
                table: "ArticleTegs",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTegs_Tegs_TegId",
                table: "ArticleTegs",
                column: "TegId",
                principalTable: "Tegs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
