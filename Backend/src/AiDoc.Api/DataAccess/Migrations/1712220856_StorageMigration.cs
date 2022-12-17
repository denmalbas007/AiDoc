using FluentMigrator;

namespace AiDoc.Api.DataAccess.Migrations;

[Migration(1712220856)]
public sealed class StorageMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("storage")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString()
            .WithColumn("url").AsString().Nullable()
            .WithColumn("content_type").AsString()
            .WithColumn("content_disposition").AsString()
            .WithColumn("bytes").AsString()
            .WithColumn("hash").AsString().Nullable();
    }
}