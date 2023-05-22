using Resources.DAL.Data;

namespace Resources.BLL.Services
{
    public class ResourcesService
    {
        private readonly ApplicationDbContext _context;

        public ResourcesService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
