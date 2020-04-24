using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.DAL;
using DecissionService.Core.DAL.IRepositories;
using DecissionService.Core.Domain;

namespace DecissionService.DataBase.Repositories
{
    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(ApiContext context) : base(context)
        {
        }
        public async Task<User> GetWithAddressAsync(string id)
        {
            return await entities
                .Where(u => u.Id == id)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync();
        }
    }
}
