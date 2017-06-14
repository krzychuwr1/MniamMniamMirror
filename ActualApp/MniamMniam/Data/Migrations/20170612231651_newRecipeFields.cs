using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MniamMniam.Data.Migrations
{
    public partial class newRecipeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailedText",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeNeeded",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailedText",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "TimeNeeded",
                table: "Recipes");
        }
    }
}
