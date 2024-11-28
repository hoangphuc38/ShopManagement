﻿using Microsoft.EntityFrameworkCore;
using ShopManagement_Backend_Core.Entities;
using System.Data.Common;

namespace ShopManagement_Backend_DataAccess.Persistance
{
    public partial class ShopManagementDbContext : DbContext
    {
        public ShopManagementDbContext()
        {
        }

        public ShopManagementDbContext(DbContextOptions<ShopManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Shop> Shops { get; set; }

        public virtual DbSet<ShopDetail> ShopDetails { get; set; }

        public virtual DbSet<Token> Tokens { get; set; }

        public virtual DbSet<User> Users { get; set; }

        //Dapper Connection
        public DbConnection GetDbConnection() => Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED860CB425");

                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.ProductName).HasMaxLength(100);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A512745A2");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.ShopId).HasName("PK__Shop__67C556292B5FFE6D");

                entity.ToTable("Shop");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ShopAddress).HasMaxLength(100);
                entity.Property(e => e.ShopName).HasMaxLength(60);
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User).WithMany(p => p.Shops)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserID");
            });

            modelBuilder.Entity<ShopDetail>(entity =>
            {
                entity.HasKey(e => new { e.ShopId, e.ProductId }).HasName("PK__ShopDeta__AC859A473E7A798A");

                entity.ToTable("ShopDetail");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product).WithMany(p => p.ShopDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductID");

                entity.HasOne(d => d.Shop).WithMany(p => p.ShopDetails)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShopID");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(e => e.TokenId).HasName("PK__Token__658FEE8A3D41F60B");

                entity.ToTable("Token");

                entity.Property(e => e.TokenId).HasColumnName("TokenID");
                entity.Property(e => e.ExpiredDate);
                entity.Property(e => e.RefreshToken).HasMaxLength(100);
                entity.Property(e => e.UserID).HasColumnName("UserID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Users__3214EC273255EA6F");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.FullName).HasMaxLength(60);
                entity.Property(e => e.Password)
                    .HasMaxLength(60)
                    .IsUnicode(false);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.SignUpDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UserName)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
