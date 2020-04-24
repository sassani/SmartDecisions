using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.DAL;
using DecissionCore.Core.DAL.IRepositories;
using DecissionCore.Core.Domain;

namespace DecissionCore.DataBase.Repositories
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
