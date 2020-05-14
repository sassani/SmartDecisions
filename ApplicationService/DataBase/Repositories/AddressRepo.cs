using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationService.Core.DAL.IRepositories;
using ApplicationService.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.DAL;

namespace ApplicationService.DataBase.Repositories
{
    public class AddressRepo : Repository<Address>, IAddressRepo
    {
        public AddressRepo(ApiContext context) : base(context)
        {
        }

        public async Task<ICollection<Address>> GetAllUserAddressesAsync(string userId)
        {
            return await entities
                .Where(a => a.OwnerId == userId)
                .ToListAsync();
        }
    }
}
