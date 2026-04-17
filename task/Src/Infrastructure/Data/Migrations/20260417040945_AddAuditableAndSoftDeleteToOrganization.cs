using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task.Src.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableAndSoftDeleteToOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrganizationUnits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "OrganizationUnits",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationUnits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrganizationUnits");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "OrganizationUnits");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "OrganizationUnits");
        }
    }
}
