﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace kid_quest_back.Migrations
{
    public partial class CreatePreviewNullableForQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Previews_PreviewId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "PreviewId",
                table: "Questions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Previews_PreviewId",
                table: "Questions",
                column: "PreviewId",
                principalTable: "Previews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Previews_PreviewId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "PreviewId",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Previews_PreviewId",
                table: "Questions",
                column: "PreviewId",
                principalTable: "Previews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
