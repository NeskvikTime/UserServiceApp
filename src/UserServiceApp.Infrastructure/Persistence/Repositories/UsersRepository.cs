﻿using Microsoft.EntityFrameworkCore;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistence.Repositories;
internal class UsersRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(user, cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(user, cancellationToken);
    }

    public Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        dbContext.Remove(user);

        return Task.CompletedTask;
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        dbContext.Update(user);

        return Task.CompletedTask;
    }

    public async Task<bool> UsernameIsUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return !await dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == name, cancellationToken);
    }

    public async Task<bool> UsernameIsUniqueAsync(Guid id, string username, CancellationToken cancellationToken)
    {
        return !await dbContext.Users
            .Where(u => u.Id != id)
            .AsNoTracking()
            .AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken)
    {
        return !await dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> EmailIsUniqueAsync(Guid id, string email, CancellationToken cancellationToken)
    {
        return !await dbContext.Users
            .Where(u => u.Id != id)
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UserByEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);
    }
}
