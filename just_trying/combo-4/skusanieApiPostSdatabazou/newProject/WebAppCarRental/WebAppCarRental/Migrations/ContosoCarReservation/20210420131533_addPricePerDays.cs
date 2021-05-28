using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppCarRental.Migrations.ContosoCarReservation
{
    public partial class addPricePerDays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePerDays",
                table: "Reservations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerDays",
                table: "Reservations");
        }
    }
}
