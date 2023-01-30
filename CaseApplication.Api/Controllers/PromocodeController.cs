using CaseApplication.DomainLayer.Repositories;

namespace CaseApplication.Api.Controllers;

public class PromocodeController
{
    private readonly IPromocodeRepository _promocodeRepository;

    public PromocodeController(IPromocodeRepository promocodeRepository)
    {
        _promocodeRepository = promocodeRepository;
    }
}