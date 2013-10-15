namespace Radvill.DataFactory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoppedToQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Stopped", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Stopped");
        }
    }
}
