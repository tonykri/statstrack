using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountService.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToEmailCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailCodes",
                table: "EmailCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "EmailCodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailCodes",
                table: "EmailCodes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmailCodes_AccountId",
                table: "EmailCodes",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailCodes",
                table: "EmailCodes");

            migrationBuilder.DropIndex(
                name: "IX_EmailCodes_AccountId",
                table: "EmailCodes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EmailCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailCodes",
                table: "EmailCodes",
                column: "AccountId");
        }
    }
}
