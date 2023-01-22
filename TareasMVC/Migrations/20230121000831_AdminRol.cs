using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(Select Id from AspNetRoles where Id = '40f8074b-5b82-467f-8e70-49f684d54640')
                                    BEGIN
	                                    INSERT AspNetRoles (Id, [Name], [NormalizedName])
	                                    VALUES ('40f8074b-5b82-467f-8e70-49f684d54640', 'admin', 'ADMIN')
                                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles Where Id = '40f8074b-5b82-467f-8e70-49f684d54640'");
        }
    }
}
