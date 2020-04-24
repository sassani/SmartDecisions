using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.DAL;
using DecissionCore.Core.DAL.IRepositories;
using DecissionCore.Core.Domain;

namespace DecissionCore.DataBase.Repositories
{
    public class AddressRepo : Repository<Address>, IAddressRepo
    {
        public AddressRepo(ApiContext context) : base(context)
        {
        }

        public async Task<ICollection<Address>> GetAllUserAddressesAsync(string userId)
        {
            return await entities
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
