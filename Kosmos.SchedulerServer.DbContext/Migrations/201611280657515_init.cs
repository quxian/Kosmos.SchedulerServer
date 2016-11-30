namespace Kosmos.SchedulerServer.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Urls",
                c => new
                    {
                        UrlHashCode = c.String(nullable: false, maxLength: 32),
                        UrlValue = c.String(),
                        IsDownloaded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UrlHashCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Urls");
        }
    }
}
