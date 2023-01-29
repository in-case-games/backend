using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IBaseRepository<T>
    {
        public Task<T> Get(Guid id);
        public Task<bool> Create(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(Guid id);
    }
}
