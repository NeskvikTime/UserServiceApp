using Microsoft.EntityFrameworkCore;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistance;
public static class DataGenerator
{
    public static ModelBuilder SeedDataToInitialMigration(this ModelBuilder modelBuilder)
    {
        var admin = new User("admin", "Admin", "admin@google.com", null!, "en", "en", "admin")
        {
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };

        admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.Password);
        admin.Password = "*******";

        modelBuilder.Entity<User>().HasData(admin);

        return modelBuilder;
    }
}
