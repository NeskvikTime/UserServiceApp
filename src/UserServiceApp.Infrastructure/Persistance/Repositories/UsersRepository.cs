using Microsoft.EntityFrameworkCore;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistance.Repositories;
internal class UsersRepository(ApplicationDbContext _dbContext) : IUsersRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(user, cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(user, cancellationToken);
    }

    public Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Remove(user);

        return Task.CompletedTask;
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Update(user);

        return Task.CompletedTask;
    }

    public async Task<bool> UsernameIsUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return !await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == name, cancellationToken);
    }

    public async Task<bool> UsernameIsUniqueAsync(Guid id, string name, CancellationToken cancellationToken)
    {
        return !await _dbContext.Users
            .Where(u => u.Id != id)
            .AsNoTracking()
            .AnyAsync(u => u.Username == name, cancellationToken);
    }

    public async Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken)
    {
        return !await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> EmailIsUniqueAsync(Guid id, string email, CancellationToken cancellationToken)
    {
        return !await _dbContext.Users
            .Where(u => u.Id != id)
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UserByEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);
    }
}
