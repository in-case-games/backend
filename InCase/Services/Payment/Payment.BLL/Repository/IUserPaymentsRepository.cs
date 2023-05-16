using MongoDB.Bson;
using Payment.DAL.Entities;

namespace Payment.BLL.Repository
{
    public interface IUserPaymentsRepository
    {
        public Task<UserPayments> GetByIdAsync(ObjectId id);
        public Task<List<UserPayments>> GetAsync(Guid userId);
    }
}
