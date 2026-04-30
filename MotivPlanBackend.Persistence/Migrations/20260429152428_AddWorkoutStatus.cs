using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotivPlanBackend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkoutStatus",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false, computedColumnSql: "upper(\"Name\")", stored: true),
                    ConcurrencyStamp = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutStatus", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutStatus",
                schema: "public");
        }
    }
}
