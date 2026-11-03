using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvarter.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kvarterContactMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    Culture = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ConsentGiven = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsHandled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kvarterContactMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kvarterNewsletterSubscriber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Culture = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ConsentGiven = table.Column<bool>(type: "INTEGER", nullable: false),
                    SubscribedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kvarterNewsletterSubscriber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kvarterWeatherForecast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LocationKey = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TemperatureMin = table.Column<double>(type: "REAL", nullable: false),
                    TemperatureMax = table.Column<double>(type: "REAL", nullable: false),
                    WeatherCode = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecipitationMm = table.Column<double>(type: "REAL", nullable: true),
                    WindSpeedMs = table.Column<double>(type: "REAL", nullable: true),
                    FetchedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kvarterWeatherForecast", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kvarterContactMessage_CreatedAtUtc",
                table: "kvarterContactMessage",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_kvarterNewsletterSubscriber_Email",
                table: "kvarterNewsletterSubscriber",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kvarterWeatherForecast_LocationKey_Date",
                table: "kvarterWeatherForecast",
                columns: new[] { "LocationKey", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kvarterContactMessage");

            migrationBuilder.DropTable(
                name: "kvarterNewsletterSubscriber");

            migrationBuilder.DropTable(
                name: "kvarterWeatherForecast");
        }
    }
}
