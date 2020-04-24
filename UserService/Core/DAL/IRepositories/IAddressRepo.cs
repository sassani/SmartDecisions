using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DAL.Interfaces;
using DecissionCore.Core.Domain;

namespace DecissionCore.Core.DAL.IRepositories
{
    public interface IAddressRepo:IRepository<Address>
    {
        Task<ICollection<Address>> GetAllUserAddressesAsync(string userId);
    }
}
