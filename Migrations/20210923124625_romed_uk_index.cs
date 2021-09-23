using Microsoft.EntityFrameworkCore.Migrations;

namespace LeftRightNet.Migrations
{
    public partial class romed_uk_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HeadLines_ValueText",
                table: "HeadLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HeadLines_ValueText",
                table: "HeadLines",
                column: "ValueText",
                unique: true);
        }
    }
}
