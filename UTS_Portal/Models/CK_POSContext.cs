using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class CK_POSContext : DbContext
    {
        public CK_POSContext()
        {
        }

        public CK_POSContext(DbContextOptions<CK_POSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Campus> Campus { get; set; }
        public virtual DbSet<Crdtrans202207> Crdtrans202207 { get; set; }
        public virtual DbSet<Cscard> Cscard { get; set; }
        public virtual DbSet<Faqs> Faqs { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<Holidays> Holidays { get; set; }
        public virtual DbSet<MenuInfos> MenuInfos { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<PreOrders> PreOrders { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Strans202207> Strans202207 { get; set; }
        public virtual DbSet<Sysvar> Sysvar { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=THIENHOA;Database=CK_POS;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campus>(entity =>
            {
                entity.Property(e => e.CampusId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Address).HasDefaultValueSql("('')");

                entity.Property(e => e.Description).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Crdtrans202207>(entity =>
            {
                entity.HasKey(e => new { e.TranDate, e.TransNum, e.TransCode, e.CardId, e.NodeId, e.GoodsId })
                    .HasName("PK_CrdTrans_202207");

                entity.Property(e => e.TransNum)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TransCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CardId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NodeId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.GoodsId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CardId2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CrdType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CustId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ForexCys)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PlcId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Post)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark).HasDefaultValueSql("('')");

                entity.Property(e => e.Repast)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RsCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TranTime)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Cscard>(entity =>
            {
                entity.HasKey(e => e.CardId)
                    .HasName("PK_Card");

                entity.Property(e => e.CardId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Address).HasDefaultValueSql("('')");

                entity.Property(e => e.Barcode)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CampusId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CanteenId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CardId2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.City).HasDefaultValueSql("('')");

                entity.Property(e => e.Class)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClassName)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Country).HasDefaultValueSql("('')");

                entity.Property(e => e.Course)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CrdType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CustId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.District).HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fname).HasDefaultValueSql("('')");

                entity.Property(e => e.Image)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Lname).HasDefaultValueSql("('')");

                entity.Property(e => e.Mobi)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name).HasDefaultValueSql("('')");

                entity.Property(e => e.NodeId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParentId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PersonId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Phone)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PlcId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Post)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrPhone)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark).HasDefaultValueSql("('')");

                entity.Property(e => e.Sex)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sibling)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Goods>(entity =>
            {
                entity.Property(e => e.GoodsId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Allergy1).HasDefaultValueSql("('')");

                entity.Property(e => e.Allergy2).HasDefaultValueSql("('')");

                entity.Property(e => e.Allergy3).HasDefaultValueSql("('')");

                entity.Property(e => e.Barcode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClsCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descript).HasDefaultValueSql("('')");

                entity.Property(e => e.EnName).HasDefaultValueSql("('')");

                entity.Property(e => e.FullName).HasDefaultValueSql("('')");

                entity.Property(e => e.GrpId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Image)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MercAcid)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MercType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OtherCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OtherName).HasDefaultValueSql("('')");

                entity.Property(e => e.Packunit)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Packunit2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Piceunit)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Post)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ref)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ShortName).HasDefaultValueSql("('')");

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SuppId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Symbol)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TaxCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpcCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.Property(e => e.Category)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ckcode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Class)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ItemCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MonthYear)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<PreOrders>(entity =>
            {
                entity.HasKey(e => new { e.UserCode, e.OrderDate, e.ItemCode, e.RepastId });

                entity.Property(e => e.UserCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ItemCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CanteenId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CkCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Class)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CustId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ModiTime)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ModiUser)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MonthYear)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PlcId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Post)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SubmitTm)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Strans202207>(entity =>
            {
                entity.HasKey(e => new { e.TransNum, e.GoodsId, e.Idx });

                entity.Property(e => e.TransNum)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.GoodsId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BaseUnit)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BuyId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CardId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CorrGsid)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CorrId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CrdType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CsId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CustId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CustaxId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ForexCys)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InvDept)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InvoiceNo)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.KitId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.KitType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MercType)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NodeId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PlcId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PosType)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Post)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ref)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RefNo)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark).HasDefaultValueSql("('')");

                entity.Property(e => e.Repast)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RsCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StaffId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TaxCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TranTime)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TransCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UnitSymb)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Sysvar>(entity =>
            {
                entity.Property(e => e.Varname)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DepCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descript).HasDefaultValueSql("('')");

                entity.Property(e => e.Inputmask)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Invalid)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Modify).HasDefaultValueSql("((1))");

                entity.Property(e => e.Type)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Value)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
