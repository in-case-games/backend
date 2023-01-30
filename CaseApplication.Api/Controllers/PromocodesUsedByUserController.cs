using CaseApplication.DomainLayer.Repositories;

namespace CaseApplication.Api.Controllers;

public class PromocodesUsedByUserController
{
    private readonly IPromocodeUserByUserRepository _promocodeUserByUserRepository;

    public PromocodesUsedByUserController(IPromocodeUserByUserRepository promocodeUserByUserRepository)
    {
        _promocodeUserByUserRepository = promocodeUserByUserRepository;
    }
}