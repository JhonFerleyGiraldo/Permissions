using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class PermissionDbContext : DbContext
    {
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        public PermissionDbContext(DbContextOptions<PermissionDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>()
                .HasOne(p => p.PermissionType)
                .WithMany(pt => pt.Permissions)
                .HasForeignKey(p => p.PermissionTypeId);
        }
    }
}
