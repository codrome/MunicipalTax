namespace MunicpalTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMunicipalityName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaxRates", "MunicipalityName", c => c.String(nullable: false, maxLength: 120));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxRates", "MunicipalityName");
        }
    }
}
