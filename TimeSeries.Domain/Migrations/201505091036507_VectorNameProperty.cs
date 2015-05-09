namespace TimeSeries.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VectorNameProperty : DbMigration
    {
        public override void Up()
        {           
            AddColumn("dbo.TimeSeries", "VectorName", c => c.String(nullable: false));            
        }
        
        public override void Down()
        {           
            DropColumn("dbo.TimeSeries", "VectorName");            
        }
    }
}
