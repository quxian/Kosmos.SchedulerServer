namespace Kosmos.SchedulerServer.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_Url : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Urls");
            AddColumn("dbo.Urls", "HashCode", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.Urls", "Value", c => c.String());
            AddColumn("dbo.Urls", "Depth", c => c.Int(nullable: false));
            AddColumn("dbo.Urls", "Domain", c => c.String());
            AddColumn("dbo.Urls", "Parent", c => c.String());
            AddPrimaryKey("dbo.Urls", "HashCode");
            CreateIndex("dbo.Urls", "Depth", name: "Depth");
            DropColumn("dbo.Urls", "UrlHashCode");
            DropColumn("dbo.Urls", "UrlValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Urls", "UrlValue", c => c.String());
            AddColumn("dbo.Urls", "UrlHashCode", c => c.String(nullable: false, maxLength: 32));
            DropIndex("dbo.Urls", "Depth");
            DropPrimaryKey("dbo.Urls");
            DropColumn("dbo.Urls", "Parent");
            DropColumn("dbo.Urls", "Domain");
            DropColumn("dbo.Urls", "Depth");
            DropColumn("dbo.Urls", "Value");
            DropColumn("dbo.Urls", "HashCode");
            AddPrimaryKey("dbo.Urls", "UrlHashCode");
        }
    }
}
