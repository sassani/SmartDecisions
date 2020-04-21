using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.DAL;
using UserService.Core.DAL.IRepositories;
using UserService.Core.Domain;

namespace UserService.DataBase.Repositories
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
