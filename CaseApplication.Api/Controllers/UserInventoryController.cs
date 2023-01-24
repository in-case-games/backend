using CaseApplication.DomainLayer.Entities;
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
        public async Task<UserInventory> GetUserInventory(Guid id) 
        { 
            return await _userInventoryRepository.GetUserInventory(id);
        }

        [HttpGet("GetAllUserInventories")]
        public async Task<IEnumerable<UserInventory>> GetAllUserInventories(Guid userId)
        {
            return await _userInventoryRepository.GetAllUserInventories(userId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserInventory(UserInventory userInventory)
        {
            return Ok(await _userInventoryRepository.CreateUserInventory(userInventory));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInventory(UserInventory userInventory)
        {
            return Ok(await _userInventoryRepository.UpdateUserInventory(userInventory));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserInventory(Guid id)
        {
            return Ok(await _userInventoryRepository.DeleteUserInventory(id));
        }
    }
}
