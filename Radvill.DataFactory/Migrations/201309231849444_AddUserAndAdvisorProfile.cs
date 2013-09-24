namespace Radvill.DataFactory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAndAdvisorProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        Created = c.DateTime(nullable: false),
                        AdvisorProfile_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AdvisorProfiles", t => t.AdvisorProfile_ID)
                .Index(t => t.AdvisorProfile_ID);
            
            CreateTable(
                "dbo.AdvisorProfiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "AdvisorProfile_ID" });
            DropForeignKey("dbo.Users", "AdvisorProfile_ID", "dbo.AdvisorProfiles");
            DropTable("dbo.AdvisorProfiles");
            DropTable("dbo.Users");
        }
    }
}
