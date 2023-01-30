using CaseApplication.DomainLayer.Repositories;

namespace CaseApplication.Api.Controllers;

public class PromocodeTypeController
{
    private readonly IPromocodeTypeRepository _promocodeTypeRepository;

    public PromocodeTypeController(IPromocodeTypeRepository promocodeTypeRepository)
    {
        _promocodeTypeRepository = promocodeTypeRepository;
    }
}