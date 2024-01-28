using Microsoft.EntityFrameworkCore;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistance;
public static class DataGenerator
{
    public static ModelBuilder SeedDataToInitialMigration(this ModelBuilder modelBuilder)
    {
        var admin = new User("admin", "Admin", "admin@localhost", null!, "en", "en", "admin")
        {
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(admin.Password);
        string hiddenPassword = "*******";

        admin.AssignPasswordAndHash(hiddenPassword, hashedPassword);

        modelBuilder.Entity<User>().HasData(admin);

        return modelBuilder;
    }
}
