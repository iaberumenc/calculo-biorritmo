namespace Calculo_Biorritmo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.accident",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        curp = c.String(),
                        fecha_accidente = c.DateTime(nullable: false),
                        residuo_fisico = c.Double(nullable: false),
                        residuo_emocional = c.Double(nullable: false),
                        residuo_intelectual = c.Double(nullable: false),
                        residuo_intuicional = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.employee",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        curp = c.String(maxLength: 50),
                        fecha_nacimiento = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.curp, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.employee", new[] { "curp" });
            DropTable("dbo.employee");
            DropTable("dbo.accident");
        }
    }
}
