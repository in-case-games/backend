﻿using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserInventoryController : ControllerBase
    {
        private readonly IUserInventoryRepository _userInventoryRepository;

        public UserInventoryController(IUserInventoryRepository userInventoryRepository)
        {
            _userInventoryRepository = userInventoryRepository;
        }

        [HttpGet]
        public async Task<UserInventory> Get(Guid id) 
        { 
            return await _userInventoryRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<UserInventory>> GetAll(Guid userId)
        {
            return await _userInventoryRepository.GetAll(userId);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserInventory userInventory)
        {
            return Ok(await _userInventoryRepository.Create(userInventory));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserInventory userInventory)
        {
            return Ok(await _userInventoryRepository.Update(userInventory));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userInventoryRepository.Delete(id));
        }
    }
}
