using Microsoft.EntityFrameworkCore;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;
using UserServiceApp.Infrastructure.Persistance.Configurations;

namespace UserServiceApp.Infrastructure.Persistance;
internal class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbInitializer
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Admin> Admins => Set<Admin>();

    public void Migrate()
    {
        base.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
