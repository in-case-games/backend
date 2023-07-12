using EmailSender.BLL.Interfaces;
using EmailSender.DAL.Data;

namespace EmailSender.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
