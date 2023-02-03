using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserTokensRepository : IBaseRepository<UserToken>
    {
        public Task<UserToken?> GetByIp(Guid userId, string ip);
        public Task<List<UserToken>> GetAll(Guid userId);
        public Task<bool> DeleteByToken(Guid userId, string token);
        public Task<bool> DeleteAll(Guid userId);
    }
}
