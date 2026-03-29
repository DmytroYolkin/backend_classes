using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace ex05_steam.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    AppId = table.Column<int>(type: "integer", nullable: false),
                    Tags = table.Column<string>(type: "jsonb", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "english")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "DetailedDescription", "AboutTheGame", "Developer", "Publisher" }),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    English = table.Column<int>(type: "integer", nullable: false),
                    Developer = table.Column<string>(type: "text", nullable: false),
                    Publisher = table.Column<string>(type: "text", nullable: false),
                    Platforms = table.Column<string>(type: "text", nullable: false),
                    RequiredAge = table.Column<int>(type: "integer", nullable: false),
                    Categories = table.Column<string>(type: "text", nullable: false),
                    Genres = table.Column<string>(type: "text", nullable: false),
                    SteamSpyTags = table.Column<string>(type: "text", nullable: false),
                    Achievements = table.Column<int>(type: "integer", nullable: false),
                    PositiveRatings = table.Column<int>(type: "integer", nullable: false),
                    NegativeRatings = table.Column<int>(type: "integer", nullable: false),
                    AveragePlaytime = table.Column<int>(type: "integer", nullable: false),
                    MedianPlaytime = table.Column<int>(type: "integer", nullable: false),
                    Owners = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DetailedDescription = table.Column<string>(type: "text", nullable: false),
                    AboutTheGame = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    Requirements = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.AppId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_SearchVector",
                table: "Games",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
