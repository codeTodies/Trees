namespace DoAnNhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NameUser = c.String(nullable: false),
                        RoleUser = c.String(nullable: false),
                        PasswordUser = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        IDArea = c.Int(nullable: false, identity: true),
                        ImgArea = c.String(nullable: false),
                        NameArea = c.String(nullable: false),
                        IDRe = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDArea)
                .ForeignKey("dbo.Regions", t => t.IDRe, cascadeDelete: true)
                .Index(t => t.IDRe);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        IDRe = c.Int(nullable: false, identity: true),
                        ImgRe = c.String(nullable: false),
                        NameRe = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDRe);
            
            CreateTable(
                "dbo.Trees",
                c => new
                    {
                        TreeID = c.Int(nullable: false, identity: true),
                        NameTree = c.String(nullable: false),
                        DescriptionTree = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageTree = c.String(nullable: false),
                        IDArea = c.Int(),
                    })
                .PrimaryKey(t => t.TreeID)
                .ForeignKey("dbo.Areas", t => t.IDArea)
                .Index(t => t.IDArea);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        IDCus = c.Int(nullable: false, identity: true),
                        NameCus = c.String(nullable: false),
                        PhoneCus = c.String(nullable: false),
                        EmailCus = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDCus);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trees", "IDArea", "dbo.Areas");
            DropForeignKey("dbo.Areas", "IDRe", "dbo.Regions");
            DropIndex("dbo.Trees", new[] { "IDArea" });
            DropIndex("dbo.Areas", new[] { "IDRe" });
            DropTable("dbo.Customers");
            DropTable("dbo.Trees");
            DropTable("dbo.Regions");
            DropTable("dbo.Areas");
            DropTable("dbo.AdminUsers");
        }
    }
}
