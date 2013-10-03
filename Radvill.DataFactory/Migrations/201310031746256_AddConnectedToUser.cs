namespace Radvill.DataFactory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConnectedToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Connected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Connected");
        }
    }
}
