namespace CaseApplication.DomainLayer.Repositories
{
    public interface IBaseRepository<T>
    {
        public Task<T?> Get(Guid id);
        public Task<bool> Create(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(Guid id);
    }
}
