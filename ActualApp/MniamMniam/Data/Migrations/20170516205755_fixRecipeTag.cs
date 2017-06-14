using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MniamMniam.Data.Migrations
{
    public partial class fixRecipeTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Ingredients_IngredientId",
                table: "RecipeTags");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "RecipeTags",
                newName: "TagId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTags_IngredientId",
                table: "RecipeTags",
                newName: "IX_RecipeTags_TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "RecipeTags",
                newName: "IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTags_TagId",
                table: "RecipeTags",
                newName: "IX_RecipeTags_IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Ingredients_IngredientId",
                table: "RecipeTags",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
