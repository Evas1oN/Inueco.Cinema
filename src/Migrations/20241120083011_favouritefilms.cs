using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inueco.Cinema.Migrations
{
    /// <inheritdoc />
    public partial class favouritefilms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilmUser",
                columns: table => new
                {
                    FavouriteByUsersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FavouriteFilmsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmUser", x => new { x.FavouriteByUsersId, x.FavouriteFilmsId });
                    table.ForeignKey(
                        name: "FK_FilmUser_Films_FavouriteFilmsId",
                        column: x => x.FavouriteFilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmUser_Users_FavouriteByUsersId",
                        column: x => x.FavouriteByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmUser_FavouriteFilmsId",
                table: "FilmUser",
                column: "FavouriteFilmsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmUser");
        }
    }
}
