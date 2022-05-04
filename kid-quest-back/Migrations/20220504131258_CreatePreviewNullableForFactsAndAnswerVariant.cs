using Microsoft.EntityFrameworkCore.Migrations;

namespace kid_quest_back.Migrations
{
    public partial class CreatePreviewNullableForFactsAndAnswerVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerVariants_Previews_PreviewId",
                table: "AnswerVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_Facts_Questions_QuestionId",
                table: "Facts");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Facts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "PreviewId",
                table: "AnswerVariants",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerVariants_Previews_PreviewId",
                table: "AnswerVariants",
                column: "PreviewId",
                principalTable: "Previews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facts_Questions_QuestionId",
                table: "Facts",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerVariants_Previews_PreviewId",
                table: "AnswerVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_Facts_Questions_QuestionId",
                table: "Facts");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Facts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PreviewId",
                table: "AnswerVariants",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerVariants_Previews_PreviewId",
                table: "AnswerVariants",
                column: "PreviewId",
                principalTable: "Previews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facts_Questions_QuestionId",
                table: "Facts",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
