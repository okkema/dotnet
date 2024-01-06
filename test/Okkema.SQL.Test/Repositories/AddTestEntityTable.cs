using FluentMigrator;
namespace Okkema.SQL.Test.Repositories;
[Migration(20231230)]
public class AddTestEntityTable : Migration
{
    public override void Up()
    {
        Create.Table("TestEntity")
            .WithColumn("Integer").AsInt32()
            .WithColumn("Long").AsInt64()
            .WithColumn("Float").AsFloat()
            .WithColumn("String").AsString()
            .WithColumn("DateTime").AsDateTime()
            .WithColumn("SystemKey").AsGuid().PrimaryKey().NotNullable().Indexed()
            .WithColumn("SystemCreatedDate").AsString().NotNullable()
            .WithColumn("SystemModifiedDate").AsString().NotNullable();
    }
    public override void Down()
    {
        Delete.Table("TestEntity");
    }
}