﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inueco.Cinema.Migrations
{
    /// <inheritdoc />
    public partial class PosterUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterUrl",
                table: "Films",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Films");
        }
    }
}
