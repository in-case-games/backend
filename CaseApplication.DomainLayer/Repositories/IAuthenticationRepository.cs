using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IAuthenticationRepository
    {
        public Task<User> SignIn(string email, string password);
        public Task<bool> SignOut(string email, string token);
       
    }
}
