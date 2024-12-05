using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopManagement_Backend_Core.Entities;
using System.Data.Common;

namespace ShopManagement_Backend_DataAccess.Persistance
{
    public partial class ShopManagementDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ShopManagementDbContext(
            DbContextOptions<ShopManagementDbContext> options,
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<NotificationRecepient> NotificationRecepients { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Shop> Shops { get; set; }

        public virtual DbSet<ShopDetail> ShopDetails { get; set; }

        public virtual DbSet<Token> Tokens { get; set; }

        public virtual DbSet<User> Users { get; set; }

        //Dapper Connection
        public DbConnection GetDbConnection()
        {
            var connectionString = _configuration.GetConnectionString("ShopManagement"); 

            if (string.IsNullOrEmpty(connectionString)) 
            { 
                throw new InvalidOperationException("The ConnectionString property has not been initialized."); 
            }

            var connection = Database.GetDbConnection(); 
            connection.ConnectionString = connectionString; 
            return connection;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32852DC955");

                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
                entity.Property(e => e.Content).HasMaxLength(500);
                entity.Property(e => e.SenderInfo).HasMaxLength(255);
                entity.Property(e => e.SentDate).HasColumnType("datetime");
                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<NotificationRecepient>(entity =>
            {
                entity.HasKey(e => e.NotificationRecepientId).HasName("PK__Notifica__BE0A1EF5EDFB4629");

                entity.ToTable("NotificationRecepient");

                entity.Property(e => e.NotificationRecepientId).HasColumnName("NotificationRecepientID");
                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Notification).WithMany(p => p.NotificationRecepients)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationID");

                entity.HasOne(d => d.User).WithMany(p => p.NotificationRecepients)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserNoti");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED860CB425");

                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.ProductName).HasMaxLength(100);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
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
                entity.HasKey(e => e.TokenId).HasName("PK__Token__658FEE8AE75FB5B3");

                entity.ToTable("Token");

                entity.Property(e => e.TokenId).HasColumnName("TokenID");
                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");
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
