using FluentMigrator;

namespace AiDoc.Api.DataAccess.Migrations;

[Migration(1712220939)]
public sealed class UserMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("users_contents")
            .WithColumn("user_id").AsGuid()
            .WithColumn("file_id").AsGuid()
            .WithColumn("name").AsString()
            .WithColumn("url").AsString()
            .WithColumn("prediction").AsString()
            .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);

        Create
            .Table("contents_sentencies_predictions")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("prediction").AsString()
            .WithColumn("file_id").AsGuid();
    }
}