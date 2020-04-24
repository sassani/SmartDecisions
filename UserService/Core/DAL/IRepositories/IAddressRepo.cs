using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DAL.Interfaces;
using DecissionService.Core.Domain;

namespace DecissionService.Core.DAL.IRepositories
{
    public interface IAddressRepo:IRepository<Address>
    {
        Task<ICollection<Address>> GetAllUserAddressesAsync(string userId);
    }
}
