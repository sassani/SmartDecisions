using System.Threading.Tasks;
using ApplicationService.Core.DAL;
using ApplicationService.Core.DAL.IRepositories;
using ApplicationService.DataBase.Repositories;

namespace ApplicationService.DataBase
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext context;

        public UnitOfWork(ApiContext context)
        {
            this.context = context;
            Profile = new ProfileRepo(context);
            Address = new AddressRepo(context);
            Avatar = new AvatarRepo(context);
        }

        public IProfileRepo Profile { get; }
        public IAddressRepo Address { get; }
        public IAvatarRepo Avatar { get; }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
