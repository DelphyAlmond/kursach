using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class newViewForStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanOfStudyProfile",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanOfStudyProfile",
                table: "Students");
        }
    }
}
