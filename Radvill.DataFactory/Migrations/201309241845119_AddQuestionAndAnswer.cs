namespace Radvill.DataFactory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestionAndAnswer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        User_ID = c.Int(),
                        Category_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .ForeignKey("dbo.Categories", t => t.Category_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Category_ID);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Accepted = c.Boolean(),
                        Question_ID = c.Int(),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Questions", t => t.Question_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.Question_ID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Answers", new[] { "User_ID" });
            DropIndex("dbo.Answers", new[] { "Question_ID" });
            DropIndex("dbo.Questions", new[] { "Category_ID" });
            DropIndex("dbo.Questions", new[] { "User_ID" });
            DropForeignKey("dbo.Answers", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Answers", "Question_ID", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.Questions", "User_ID", "dbo.Users");
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
        }
    }
}
