﻿using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public RoleController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        [HttpGet]
        public async Task<UserRole> Get(Guid id)
        {
            return await _userRoleRepository.Get(id);
        }

        [HttpGet("GetByRole")]
        public async Task<UserRole> GetByRole(Guid roleId = new(), string? roleName = null)
        {
            UserRole role = new() { Id = roleId, RoleName = roleName };
            return await _userRoleRepository.GetByRole(role);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<UserRole>> GetAll()
        {
            return await _userRoleRepository.GetAll();
        }

        [HttpPost]
        public async Task<bool> Create(UserRole userRole)
        {
            return await _userRoleRepository.Create(userRole);
        }

        [HttpPut]
        public async Task<bool> Update(UserRole userRole)
        {
            return await _userRoleRepository.Update(userRole);
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            return await _userRoleRepository.Delete(id);
        }
    }
}
