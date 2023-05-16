using MongoDB.Bson;
using MongoDB.Driver;
using Payment.DAL.Entities;

namespace Payment.BLL.Repository
{
    public class UserPaymentsRepository : IUserPaymentsRepository
    {
        private readonly IMongoCollection<UserPayments> _userPayments;

        public UserPaymentsRepository(IMongoClient client)
        {
            IMongoDatabase database = client.GetDatabase("Payments");

            _userPayments = database.GetCollection<UserPayments>("UserPayments");
        }

        public Task<UserPayments> GetByIdAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserPayments>> GetAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
