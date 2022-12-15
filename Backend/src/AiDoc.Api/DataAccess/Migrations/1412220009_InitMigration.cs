using FluentMigrator;

namespace AiDoc.Api.DataAccess.Migrations;

[Migration(1412220009)]
public sealed class InitMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("password").AsString()
            .WithColumn("email").AsString().Unique()
            .WithColumn("profile_pic_url").AsString().Nullable()
            .WithColumn("full_name").AsString();
    }
}