using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorePGIdentityTest.Migrations
{
    public partial class Index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_weather_forecasts_id",
                table: "weather_forecasts",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_weather_forecasts_id",
                table: "weather_forecasts");
        }
    }
}
