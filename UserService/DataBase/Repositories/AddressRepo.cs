using Shared.DAL;
using UserService.Core.DAL.IRepositories;
using UserService.Core.Domain;

namespace UserService.DataBase.Repositories
{
    public class AddressRepo : Repository<Address>, IAddressRepo
    {
        public AddressRepo(ApiContext context) : base(context)
        {
        }
    }
}
