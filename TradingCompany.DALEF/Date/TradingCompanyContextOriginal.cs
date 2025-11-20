using Microsoft.EntityFrameworkCore;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Date
{
    public class TradingCompanyContextOriginal : DbContext
    {
        public TradingCompanyContextOriginal() { }
        public TradingCompanyContextOriginal(DbContextOptions<TradingCompanyContextOriginal> options) 
            : base(options) 
        {
        }
        public DbSet<LogModels> Logs { get; set; }
        public DbSet<OrderModels> Orders { get; set; }
        public DbSet<RoleModels> Roles { get; set; }

        public DbSet<ShipmentModels> Shipments { get; set; }
        public DbSet<StatusModels> Statuses { get; set; }
        public DbSet<UserModels> Users { get; set; }
        public DbSet<UserRoleModels> UserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("karoche");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogModels>(entity =>
            {
                entity.HasOne(l => l.User).WithMany(u => u.Logs).HasConstraintName("FK_Logs_Users2");
            });
            modelBuilder.Entity<OrderModels>(entity =>
            {
                entity.HasOne(o => o.Status).WithMany(s => s.Orders).HasConstraintName("FK_Orders_Statuses");
            });
            modelBuilder.Entity<ShipmentModels>(entity =>
            {
                entity.HasOne(s => s.Order).WithMany(o => o.Shipments).HasConstraintName("FK_Shipments_Orders");
            });
            modelBuilder.Entity<UserRoleModels>(entity =>
            {
                entity.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasConstraintName("FK_User_Roles_Users");
            });
            modelBuilder.Entity<UserRoleModels>(entity =>
            {
                entity.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasConstraintName("FK_User_Roles_Roles");
            });
        }

    }
}
