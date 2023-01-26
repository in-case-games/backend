using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface INewsRepository
    {
        public Task<News> Get(Guid id);
        public Task<IEnumerable<News>> GetAll();
        public Task<bool> Create(News news);
        public Task<bool> Update(News news);
        public Task<bool> Delete(Guid id);
    }
}
