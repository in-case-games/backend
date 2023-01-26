﻿using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserHistoryOpeningCasesController : ControllerBase
    {
        private readonly IUserHistoryOpeningCasesRepository _userHistoryRepository;

        public UserHistoryOpeningCasesController(IUserHistoryOpeningCasesRepository userHistoryRepository)
        {
            _userHistoryRepository = userHistoryRepository;
        }

        [HttpGet]
        public async Task<UserHistoryOpeningCases> Get(Guid id)
        {
            return await _userHistoryRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<UserHistoryOpeningCases>> GetAll(Guid userId)
        {
            return await _userHistoryRepository.GetAll(userId);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserHistoryOpeningCases userHistory)
        {
            return Ok(await _userHistoryRepository.Create(userHistory));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserHistoryOpeningCases userHistory)
        {
            return Ok(await _userHistoryRepository.Update(userHistory));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userHistoryRepository.Delete(id));
        }

        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAll(Guid userId)
        {
            return Ok(await _userHistoryRepository.DeleteAll(userId));
        }
    }
}
