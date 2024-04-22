﻿using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using xPlanner.Domain.Entities;

namespace xPlanner.Data.Repository;

public class UserRepository : IRepository<User>
{
    private readonly AppDbContext dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        try
        {
            User existed = await GetById(user.Id);
        } 
        catch (ObjectNotFoundException)
        {
            user.Id = dbContext.Users.Count();
            user.CreatedAt = DateTime.UtcNow;
            user.Name = user.Email;
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            User user = await GetById(id);

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception) { throw; }
    }

    public async Task<List<User>> GetAll()
    {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User> GetById(int id)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == id) ??
            throw new ObjectNotFoundException();
    }

    public async Task<User> GetByEmail(string email)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email) ??
            throw new ObjectNotFoundException();
    } 

    public async Task Update(User user)
    {
        try
        {
            User existed = await GetById(user.Id);

            user.LastUpdatedAt = DateTime.Now;
            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
        catch (Exception) { throw; }
    }
}
