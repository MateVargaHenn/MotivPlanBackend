using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotivPlanBackend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProfileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                schema: "public",
                table: "WorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                schema: "public",
                table: "WorkoutExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutExercises",
                schema: "public",
                table: "WorkoutExercises");

            migrationBuilder.RenameTable(
                name: "WorkoutExercises",
                schema: "public",
                newName: "WorkoutExercise",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                schema: "public",
                table: "WorkoutExercise",
                newName: "IX_WorkoutExercise_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutExercise",
                schema: "public",
                table: "WorkoutExercise",
                columns: new[] { "WorkoutId", "ExerciseId" });

            migrationBuilder.CreateTable(
                name: "Preferences",
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
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
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
                    table.PrimaryKey("PK_Sex", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SexId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Sex_SexId",
                        column: x => x.SexId,
                        principalSchema: "public",
                        principalTable: "Sex",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePreference",
                schema: "public",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "integer", nullable: false),
                    PreferenceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePreference", x => new { x.ProfileId, x.PreferenceId });
                    table.ForeignKey(
                        name: "FK_ProfilePreference_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "public",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfilePreference_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "public",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePreference_PreferenceId",
                schema: "public",
                table: "ProfilePreference",
                column: "PreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_SexId",
                schema: "public",
                table: "Profiles",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                schema: "public",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercise_Exercises_ExerciseId",
                schema: "public",
                table: "WorkoutExercise",
                column: "ExerciseId",
                principalSchema: "public",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercise_Workouts_WorkoutId",
                schema: "public",
                table: "WorkoutExercise",
                column: "WorkoutId",
                principalSchema: "public",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercise_Exercises_ExerciseId",
                schema: "public",
                table: "WorkoutExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercise_Workouts_WorkoutId",
                schema: "public",
                table: "WorkoutExercise");

            migrationBuilder.DropTable(
                name: "ProfilePreference",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Sex",
                schema: "public");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutExercise",
                schema: "public",
                table: "WorkoutExercise");

            migrationBuilder.RenameTable(
                name: "WorkoutExercise",
                schema: "public",
                newName: "WorkoutExercises",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutExercise_ExerciseId",
                schema: "public",
                table: "WorkoutExercises",
                newName: "IX_WorkoutExercises_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutExercises",
                schema: "public",
                table: "WorkoutExercises",
                columns: new[] { "WorkoutId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                schema: "public",
                table: "WorkoutExercises",
                column: "ExerciseId",
                principalSchema: "public",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                schema: "public",
                table: "WorkoutExercises",
                column: "WorkoutId",
                principalSchema: "public",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
