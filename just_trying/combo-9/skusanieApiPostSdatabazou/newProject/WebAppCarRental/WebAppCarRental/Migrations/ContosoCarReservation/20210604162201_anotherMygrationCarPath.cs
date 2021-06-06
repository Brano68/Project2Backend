using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppCarRental.Migrations.ContosoCarReservation
{
    public partial class anotherMygrationCarPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Cars");
        }
    }
}
