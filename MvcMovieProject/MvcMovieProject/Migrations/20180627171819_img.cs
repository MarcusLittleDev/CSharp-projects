using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MvcMovieProject.Migrations
{
    public partial class img : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Actor");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MovieRole",
                newName: "Character");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Actor",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Movie",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthName",
                table: "Actor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "BirthName",
                table: "Actor");

            migrationBuilder.RenameColumn(
                name: "Character",
                table: "MovieRole",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Actor",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Actor",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
