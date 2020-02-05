using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SuitAlterations.Infrastructure.Migrations
{
	public partial class InitialDbCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "dbo");

			migrationBuilder.CreateTable(
				name: "Customers",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					FirstName = table.Column<string>(maxLength: 50, nullable: true),
					LastName = table.Column<string>(maxLength: 50, nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_Customers", x => x.Id); });

			migrationBuilder.CreateTable(
				name: "SuitAlterations",
				schema: "dbo",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					LeftSleeveLength = table.Column<int>(nullable: false),
					RightSleeveLength = table.Column<int>(nullable: false),
					LeftTrouserLength = table.Column<int>(nullable: false),
					RightTrouserLength = table.Column<int>(nullable: false),
					Status = table.Column<byte>(nullable: false),
					CustomerId = table.Column<Guid>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SuitAlterations", x => x.Id);
					table.ForeignKey(
						name: "FK_SuitAlterations_Customers_CustomerId",
						column: x => x.CustomerId,
						principalSchema: "dbo",
						principalTable: "Customers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_SuitAlterations_CustomerId",
				schema: "dbo",
				table: "SuitAlterations",
				column: "CustomerId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "SuitAlterations",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "Customers",
				schema: "dbo");
		}
	}
}