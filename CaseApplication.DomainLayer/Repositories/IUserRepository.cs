﻿using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRepository {
        public Task<User?> Get(Guid id);
        public Task<User?> GetByEmail(string email);
        public Task<User?> GetByLogin(string login);
        public Task<User?> GetByParameters(UserDto user);
        public Task<List<User>> GetAll();
        public Task<bool> IsUniqueSalt(string salt);
        public Task<bool> Create(UserDto userDto);
        public Task<bool> Update(UserDto oldUserDto, UserDto newUserDto);
        public Task<bool> Delete(Guid id);
    }
}
