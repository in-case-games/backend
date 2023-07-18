﻿using Infrastructure.MassTransit.User;

namespace Promocode.BLL.Interfaces
{
    public interface IUserService
    {
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
