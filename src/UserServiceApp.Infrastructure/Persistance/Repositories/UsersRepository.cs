﻿using Microsoft.EntityFrameworkCore;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistance.Repositories;
internal class UsersRepository(ApplicationDbContext _dbContext) : IUsersRepository
{
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
        return await _dbContext
            .Users
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Update(user);

        return Task.CompletedTask;
    }
}
