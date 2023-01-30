using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers;

public class PromocodeTypeController
{
    private readonly IPromocodeTypeRepository _promocodeTypeRepository;

    public PromocodeTypeController(IPromocodeTypeRepository promocodeTypeRepository)
    {
        _promocodeTypeRepository = promocodeTypeRepository;
    }

    [HttpGet]
    public async Task<PromocodeType> Get(Guid id)
    {
        return await _promocodeTypeRepository.Get(id);
    }

    [HttpGet("GetByName")]
    public async Task<PromocodeType> GetByName(string name)
    {
        return await _promocodeTypeRepository.GetByName(name);
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<PromocodeType>> GetAll()
    {
        return await _promocodeTypeRepository.GetAll();
    }

    [HttpPost]
    public async Task<bool> Create(PromocodeType promocodeType)
    {
        return await _promocodeTypeRepository.Create(promocodeType);
    }

    [HttpPut]
    public async Task<bool> Update(PromocodeType promocodeType)
    {
        return await _promocodeTypeRepository.Update(promocodeType);
    }

    [HttpDelete]
    public async Task<bool> Delete(Guid id)
    {
        return await _promocodeTypeRepository.Delete(id);
    }
}