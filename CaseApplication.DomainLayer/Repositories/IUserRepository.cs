﻿using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User> GetByParameters(User user);
        public Task<User> GetByEmail(string email);
        public Task<User> GetByLogin(string login);
        public Task<bool> IsUniqueSalt(string salt);
        public Task<IEnumerable<User>> GetAll();
    }
}
