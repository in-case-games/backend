﻿using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public UserRepository(IDbContextFactory<ApplicationDbContext> context)
        {
            _contextFactory = context;
        }
        public async Task<User> GetUser(string email)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.User.FirstOrDefaultAsync(x => x.UserEmail == email) 
                ?? throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.User.ToListAsync();
        }

        public async Task<bool> CreateUser(User user)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            user.Id = Guid.NewGuid();

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUser(User user)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            User? searchUser = await _context.User.FirstOrDefaultAsync(x => x.Id == user.Id);

            if(searchUser is null) throw new Exception("You cannot change the guid, " +
                "review what data comes from the api");

            _context.Entry(searchUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteUser(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            User? searchUser = await _context.User.FirstOrDefaultAsync(x => x.Id == id);

            if (searchUser is null) throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            _context.User.Remove(searchUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
