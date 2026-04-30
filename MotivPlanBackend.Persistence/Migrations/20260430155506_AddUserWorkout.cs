using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotivPlanBackend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schedule",
                schema: "public",
                table: "Workouts");

            migrationBuilder.CreateTable(
                name: "UserWorkout",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WorkoutId = table.Column<int>(type: "integer", nullable: false),
                    WorkoutStatus = table.Column<int>(type: "integer", nullable: false),
                    Schedule = table.Column<DateOnly>(type: "date", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWorkout_WorkoutStatus_WorkoutStatus",
                        column: x => x.WorkoutStatus,
                        principalSchema: "public",
                        principalTable: "WorkoutStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWorkout_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalSchema: "public",
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkout_WorkoutId",
                schema: "public",
                table: "UserWorkout",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkout_WorkoutStatus",
                schema: "public",
                table: "UserWorkout",
                column: "WorkoutStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWorkout",
                schema: "public");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Schedule",
                schema: "public",
                table: "Workouts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
