using System.Threading.Tasks;
using DecissionService.Core.DAL;
using DecissionService.Core.DAL.IRepositories;
using DecissionService.DataBase.Repositories;

namespace DecissionService.DataBase
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext context;

        public UnitOfWork(ApiContext context)
        {
            this.context = context;
            User = new UserRepo(context);
            Address = new AddressRepo(context);
        }

        public IUserRepo User { get; }
        public IAddressRepo Address { get; }

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
