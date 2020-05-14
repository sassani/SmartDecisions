using System.Linq;
using System.Threading.Tasks;
using ApplicationService.Core.DAL.IRepositories;
using ApplicationService.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.DAL;

namespace ApplicationService.DataBase.Repositories
{
    public class ProfileRepo : Repository<Profile>, IProfileRepo
    {
        public ProfileRepo(ApiContext context) : base(context)
        {
        }
        public async Task<Profile> GetWithAddressAsync(string id)
        {
            return await entities
                .Where(u => u.OwnerId == id)
                .Include(u => u.Addresses)
                .Include(u=>u.Avatar)
                .FirstOrDefaultAsync();
        }
    }
}
