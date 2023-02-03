using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserInventoryController : ControllerBase
    {
        private readonly IUserInventoryRepository _userInventoryRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserInventoryController(IUserInventoryRepository userInventoryRepository)
        {
            _userInventoryRepository = userInventoryRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id) 
        {
            UserInventory? userInventory = await _userInventoryRepository.Get(id);

            if(userInventory != null)
            {
                return Ok(userInventory);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid? userId = null)
        {
            return Ok(await _userInventoryRepository.GetAll(userId ?? UserId));
        }

        //TODO Sell and Withdrawn
    }
}
