using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UTS_Portal.Models
{
    public partial class db_utsContext : DbContext
    {
        public db_utsContext()
        {
        }

        public db_utsContext(DbContextOptions<db_utsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Faqs> Faqs { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<Holidays> Holidays { get; set; }
        public virtual DbSet<MenuInfos> MenuInfos { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<PreOrders> PreOrders { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=THIENHOA;Database=db_uts;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedbacks>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
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

                entity.Property(e => e.Ckcode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Class)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CustId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedTime)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedUser)
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

                entity.Property(e => e.SubmittedTime)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Users>(entity =>
            {
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
