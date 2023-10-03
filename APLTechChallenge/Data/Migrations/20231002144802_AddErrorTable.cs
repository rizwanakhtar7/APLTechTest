using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APLTechChallenge.Migrations
{
    public partial class AddErrorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErrorRecord",
                table: "ImageAudits",
                newName: "SuccessRecord");

            migrationBuilder.CreateTable(
                name: "ErrorAuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfflineMode = table.Column<bool>(type: "bit", nullable: false),
                    SentToAzure = table.Column<bool>(type: "bit", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorAuditLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorAuditLogs");

            migrationBuilder.RenameColumn(
                name: "SuccessRecord",
                table: "ImageAudits",
                newName: "ErrorRecord");
        }
    }
}
