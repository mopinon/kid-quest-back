using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace kid_quest_back.Migrations
{
    public partial class AddQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PreviewId",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_PreviewId",
                table: "Questions",
                column: "PreviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TypeId",
                table: "Questions",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Previews_PreviewId",
                table: "Questions",
                column: "PreviewId",
                principalTable: "Previews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTypes_TypeId",
                table: "Questions",
                column: "TypeId",
                principalTable: "QuestionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Previews_PreviewId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTypes_TypeId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Questions_PreviewId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TypeId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "PreviewId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Questions");
        }
    }
}
