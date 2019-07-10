namespace Variedades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableproductgarantia : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Producto", "Garantia", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Producto", "Garantia", c => c.Int(nullable: false));
        }
    }
}
