using MongoDB.Bson;
using Payment.BLL.Helpers;
using Payment.BLL.Models;
using Payment.BLL.Repository;
using Payment.DAL.Entities;

namespace Payment.BLL.Services
{
    public class UserPaymentsService : IUserPaymentsService
    {
        private readonly IUserPaymentsRepository _userPaymentsRepository;

        public UserPaymentsService(IUserPaymentsRepository userPaymentsRepository)
        {
            _userPaymentsRepository = userPaymentsRepository;
        }

        public async Task<UserPaymentsResponse> GetByIdAsync(string id)
        {
            ObjectId objectId = ObjectId.Parse(id);

            UserPayments payment = await _userPaymentsRepository.GetByIdAsync(objectId);

            return payment.ToResponse();
        }

        public async Task<List<UserPaymentsResponse>> GetAsync(Guid userId)
        {
            List<UserPayments> payments = await _userPaymentsRepository.GetAsync(userId);

            return payments.ToResponse();
        }
    }
}
