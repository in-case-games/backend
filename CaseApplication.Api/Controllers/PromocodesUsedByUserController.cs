using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers 
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodesUsedByUserController
    {
        private readonly IPromocodeUserByUserRepository _promocodeUserByUserRepository;

        public PromocodesUsedByUserController(IPromocodeUserByUserRepository promocodeUserByUserRepository)
        {
            _promocodeUserByUserRepository = promocodeUserByUserRepository;
        }

        [HttpGet]
        public async Task<PromocodesUsedByUser> Get(Guid id)
        {
            return await _promocodeUserByUserRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<PromocodesUsedByUser>> GetAll(Guid userId)
        {
            return await _promocodeUserByUserRepository.GetAll(userId);
        }

        [HttpPost]
        public async Task<bool> Create(PromocodesUsedByUser promocodesUsedByUser)
        {
            return await _promocodeUserByUserRepository.Create(promocodesUsedByUser);
        }

        [HttpPut]
        public async Task<bool> Update(PromocodesUsedByUser promocodesUsedByUser)
        {
            return await _promocodeUserByUserRepository.Update(promocodesUsedByUser);
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            return await _promocodeUserByUserRepository.Delete(id);
        }
    }
}