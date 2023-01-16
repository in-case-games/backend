using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface IAuthorizationRepository
    {
        public Task<User> SignIn(string email, string password);
    }
}
