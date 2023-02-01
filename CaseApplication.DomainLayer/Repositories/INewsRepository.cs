using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface INewsRepository : IBaseRepository<News>
    {
        public Task<List<News>> GetAll();
    }
}
