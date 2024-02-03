using Microsoft.EntityFrameworkCore;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistance;
public static class DataGenerator
{
    public static ModelBuilder SeedDataToInitialMigration(this ModelBuilder modelBuilder)
    {
        DateTime dateTime = DateTime.UtcNow;

        var admin = new User("admin",
            "Admin",
            "admin@localhost",
            "+65467891324586",
            "English",
            "en-US",
            true)
        {
            DateCreated = dateTime,
            DateModified = dateTime,
        };

        string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin-1234!");

        admin.AssignPasswordHash(hashedPassword);

        modelBuilder.Entity<User>()
            .HasData(admin);

        return modelBuilder;
    }
}
