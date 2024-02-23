using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[AspNetRoles] (Id,                                     Name,      NormalizedName)
                                         VALUES ('6E18233D-845C-4558-8BA1-4EE8C9C0724A', 'Admin',   'ADMIN'),
                                                (NEWID(),                                'UpPoint', 'UPPOINT');

                INSERT INTO [dbo].[AspNetUsers] (id,									 create_by,								 create_time, update_by,							  update_time, entity_status, active, UserName, NormalizedUserName, Email,				 NormalizedEmail,	  EmailConfirmed, PasswordHash,											   							      SecurityStamp,					  ConcurrencyStamp,						  PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
						                 VALUES ('9E11BD31-6C7C-4FAE-D54E-08DC33CA164A', '00000000-0000-0000-0000-000000000000', GETDATE(),   '00000000-0000-0000-0000-000000000000', GETDATE(),   'Use',		  1,	  'admin',  'ADMIN',			'admin@example.com', 'ADMIN@EXAMPLE.COM', 1,			  'AQAAAAIAAYagAAAAEO46bhroSIUSXeEZdTHh6lvCxdyH6/QKbnMBWWwFFEyDpA4xtBO0svEOE3Ix6SFiUA==', 'XYSAO2MGR6IYQHEXALP4M73DLIAFUHF2', '70de0c94-43f1-4c59-97ed-a07bf2e5b5e9', NULL,		   0,					 0,				   NULL,	   1,			   0);

                INSERT INTO [dbo].[AspNetUserRoles] (UserId,                                 RoleId)
							                 VALUES ('9E11BD31-6C7C-4FAE-D54E-08DC33CA164A', '6E18233D-845C-4558-8BA1-4EE8C9C0724A');

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
