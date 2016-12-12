namespace Kosmos.SchedulerServer.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_DownloadErrorUrl_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DownloadErrorUrls",
                c => new
                    {
                        UrlHashCode = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.UrlHashCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DownloadErrorUrls");
        }
    }
}
