namespace Gestor_Asignaturas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Asignaturas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Codigo = c.String(nullable: false, maxLength: 7),
                        Creditos = c.Int(nullable: false),
                        CD = c.Int(nullable: false),
                        CP = c.Int(nullable: false),
                        AA = c.Int(nullable: false),
                        Horas = c.Int(nullable: false),
                        Area = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Asignaturas");
        }
    }
}
