using Microsoft.EntityFrameworkCore;

using System.Reflection;

using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;
using UserServiceApp.Infrastructure.Persistence.Configurations;

namespace UserServiceApp.Infrastructure.Persistence;
internal class ApplicationDbContext : DbContext, IDbInitializer
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users => Set<User>();

    public void Migrate()
    {
        base.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SeedDataToInitialMigration();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
