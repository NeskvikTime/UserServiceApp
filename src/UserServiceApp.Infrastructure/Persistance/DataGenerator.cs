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
            "Admin-1234!",
            true)
        {
            DateCreated = dateTime,
            DateModified = dateTime,
        };

        string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(admin.Password);

        admin.AssignPasswordAndHash(admin.Password, hashedPassword);

        modelBuilder.Entity<User>().HasData(admin);

        return modelBuilder;
    }
}
