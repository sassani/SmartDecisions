using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationService.Core.DAL.IRepositories;
using ApplicationService.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.DAL;

namespace ApplicationService.DataBase.Repositories
{
    public class ContactRepo : Repository<Contact>, IContactRepo
    {
        public ContactRepo(ApiContext context) : base(context)
        {
        }

        public async Task<ICollection<Contact>> GetAllUserAddressesAsync(string userId)
        {
            return await entities
                .Where(a => a.OwnerId == userId)
                .ToListAsync();
        }
    }
}
