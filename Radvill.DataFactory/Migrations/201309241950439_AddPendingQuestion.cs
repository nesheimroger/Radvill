namespace Radvill.DataFactory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPendingQuestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PendingQuestions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Status = c.Boolean(),
                        TimeStamp = c.DateTime(nullable: false),
                        Question_ID = c.Int(),
                        Answer_ID = c.Int(),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Questions", t => t.Question_ID)
                .ForeignKey("dbo.Answers", t => t.Answer_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.Question_ID)
                .Index(t => t.Answer_ID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PendingQuestions", new[] { "User_ID" });
            DropIndex("dbo.PendingQuestions", new[] { "Answer_ID" });
            DropIndex("dbo.PendingQuestions", new[] { "Question_ID" });
            DropForeignKey("dbo.PendingQuestions", "User_ID", "dbo.Users");
            DropForeignKey("dbo.PendingQuestions", "Answer_ID", "dbo.Answers");
            DropForeignKey("dbo.PendingQuestions", "Question_ID", "dbo.Questions");
            DropTable("dbo.PendingQuestions");
        }
    }
}
