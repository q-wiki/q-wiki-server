using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class AddedQuestionmarkAndFullStopToTaskDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                column: "TaskDescription",
                value: "Which animal is in the image?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "TaskDescription",
                value: "Sort these actors by the number of movies they appeared in.");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                column: "TaskDescription",
                value: "Which animal is this?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                column: "TaskDescription",
                value: "Which animal is {0}?");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort these sports by participating players.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("025286ac-d6d1-4e9f-954c-f659e83d7d6d"),
                column: "TaskDescription",
                value: "Which animal is in the image");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1f2679f4-db27-47db-af4c-d2cf25708254"),
                column: "TaskDescription",
                value: "Which animal is {0}");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("3803ddc5-f8ea-4bd6-93a2-855407f8178f"),
                column: "TaskDescription",
                value: "Sort these actors by the number of movies they appeared in");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("428ac495-541e-48a9-82a2-f94a503a4f26"),
                column: "TaskDescription",
                value: "Which animal is {0}");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5abd274b-0826-4a30-832b-9e072a2cd0a4"),
                column: "TaskDescription",
                value: "Which animal is this");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b3778c74-3284-4518-a8f0-deea5a2b8363"),
                column: "TaskDescription",
                value: "Which animal is {0}");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d15f9f1c-9433-4964-a5e3-4e69ed0b45a9"),
                column: "TaskDescription",
                value: "Which animal is {0}");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("edeefc69-f882-46f6-96cd-ae9212fdb0df"),
                column: "TaskDescription",
                value: "Sort sports by participating players?");
        }
    }
}
