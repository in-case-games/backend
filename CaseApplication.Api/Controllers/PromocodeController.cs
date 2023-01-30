using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers;

public class PromocodeController
{
    private readonly IPromocodeRepository _promocodeRepository;

    public PromocodeController(IPromocodeRepository promocodeRepository)
    {
        _promocodeRepository = promocodeRepository;
    }

    [HttpGet]
    public async Task<Promocode> Get(Guid id)
    {
        return await _promocodeRepository.Get(id);
    }

    [HttpGet("GetByName")]
    public async Task<Promocode> GetByName(string name)
    {
        return await _promocodeRepository.GetByName(name);
    }

    [HttpPost]
    public async Task<bool> Create(Promocode promocode)
    {
        return await _promocodeRepository.Create(promocode);
    }

    [HttpPut]
    public async Task<bool> Update(Promocode promocode)
    {
        return await _promocodeRepository.Update(promocode);
    }

    [HttpDelete]
    public async Task<bool> Delete(Guid id)
    {
        return await _promocodeRepository.Delete(id);
    }
}