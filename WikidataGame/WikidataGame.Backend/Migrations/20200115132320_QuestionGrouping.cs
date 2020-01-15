using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WikidataGame.Backend.Migrations
{
    public partial class QuestionGrouping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Questions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("0d218830-55d2-4d66-8d8f-d402514e9202"),
                column: "GroupId",
                value: new Guid("d375ff0f-cb79-4eac-84e6-c4bf65c2382a"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("14d93797-c61c-4415-b1ed-359d180237ff"),
                column: "GroupId",
                value: new Guid("98a751f6-eee0-4d79-9401-992417283aa9"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("29fed1d0-d306-4946-8109-63b8aaf0262e"),
                column: "GroupId",
                value: new Guid("f88bb7ba-a1dc-45c1-8c6f-1c918bf87217"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("2ed01768-9ab6-4895-8cbf-09dbc6f957e0"),
                column: "GroupId",
                value: new Guid("3de1256d-f8d2-4418-a932-d459d5ee44d6"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("40677b0f-9d5f-46d2-ab85-a6c40afb5f87"),
                column: "GroupId",
                value: new Guid("a9c75969-eacf-46d1-bf29-ad5591af98e9"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("46679c4f-ef97-445d-9a70-d95a5337720f"),
                column: "GroupId",
                value: new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("4f6c477e-7025-44b4-a3b0-f3ebd8902902"),
                column: "GroupId",
                value: new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("5f7e813a-3cfa-4617-86d1-514b481b37a8"),
                column: "GroupId",
                value: new Guid("72b3fa13-3526-4bd5-964c-442a3f3a5d31"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("8273acfe-c278-4cd4-92f5-07dd73a22577"),
                column: "GroupId",
                value: new Guid("ba587fa0-9601-4d99-a56b-7e92a5ccbe13"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("86b64102-8074-4c4e-8f3e-71a0e52bb261"),
                column: "GroupId",
                value: new Guid("976e1e61-3b95-43bf-8e4c-1963b6795113"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("909182d1-4ae6-46ea-bd9b-8c4323ea53fa"),
                column: "GroupId",
                value: new Guid("2af47804-eeaa-4bcd-98e3-f515aeaf30b5"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9a70639b-3447-475a-905a-e866a0c98a1c"),
                column: "GroupId",
                value: new Guid("e1a1fbf7-850f-4fdb-878c-9e8190b54d6b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4a7289a-3053-4ad7-9c60-c75a18305243"),
                column: "GroupId",
                value: new Guid("6cfc621c-7a35-464a-80cd-3937a6d2af3d"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a4b7c4ba-6acb-4f9a-821b-7a44aa7b6761"),
                column: "GroupId",
                value: new Guid("e6ec8ea0-39ee-476c-81f5-b17bd99e715f"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a6a470de-9efb-4fde-9388-6eb20f2ff1f4"),
                column: "GroupId",
                value: new Guid("3c750fe4-4980-46cf-b6e9-876e8228945b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("aca0f5f7-b000-42fb-b713-f5fe43748761"),
                column: "GroupId",
                value: new Guid("ffd0f0da-b31d-4c01-b946-8b81fa30b00e"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bba18c92-47a6-4541-9305-d6453ad8477a"),
                column: "GroupId",
                value: new Guid("f5d0100f-a7bf-4d6d-9767-b5a4463daeb5"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("bc7a22ee-4985-44c3-9388-5c7dd6b8762e"),
                column: "GroupId",
                value: new Guid("15f9b57e-118a-4448-b24f-b66806197ff8"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d135088c-e062-4016-8eb4-1d68c72915ea"),
                column: "GroupId",
                value: new Guid("fc74c29e-b4a9-428b-96c9-b41127869a31"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d9011896-04e5-4d32-8d3a-02a6d2b0bdb6"),
                column: "GroupId",
                value: new Guid("184f3c3b-a831-4a0b-8c01-a846608f139b"));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e8f99165-baa3-47b2-be35-c42ab2d5f0a0"),
                column: "GroupId",
                value: new Guid("62f3426e-6c47-43de-b9fc-db3e8d988986"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Questions");
        }
    }
}
