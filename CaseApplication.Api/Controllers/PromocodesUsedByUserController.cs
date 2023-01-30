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
    }
}