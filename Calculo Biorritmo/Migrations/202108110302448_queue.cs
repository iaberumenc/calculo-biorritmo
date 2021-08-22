namespace Calculo_Biorritmo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class queue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PendingSync",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        modelo = c.String(),
                        id_Object = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PendingSync");
        }
    }
}
