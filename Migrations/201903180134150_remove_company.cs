namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_company : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cliente", "Compania_IdCompania", "dbo.Compania");
            DropIndex("dbo.Cliente", new[] { "Compania_IdCompania" });
            AddColumn("dbo.Cliente", "Compania", c => c.String());
            AddColumn("dbo.Cliente", "Fecha_Pago", c => c.DateTime());
            DropColumn("dbo.Cliente", "Compania_IdCompania");
            DropTable("dbo.Compania");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Compania",
                c => new
                    {
                        IdCompania = c.Int(nullable: false, identity: true),
                        Trabajo = c.String(),
                        Fecha_Pago = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdCompania);
            
            AddColumn("dbo.Cliente", "Compania_IdCompania", c => c.Int());
            DropColumn("dbo.Cliente", "Fecha_Pago");
            DropColumn("dbo.Cliente", "Compania");
            CreateIndex("dbo.Cliente", "Compania_IdCompania");
            AddForeignKey("dbo.Cliente", "Compania_IdCompania", "dbo.Compania", "IdCompania");
        }
    }
}
