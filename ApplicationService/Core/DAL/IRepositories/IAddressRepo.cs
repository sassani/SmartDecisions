using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationService.Core.Domain;
using Shared.DAL.Interfaces;

namespace ApplicationService.Core.DAL.IRepositories
{
    public interface IAddressRepo:IRepository<Address>
    {
        Task<ICollection<Address>> GetAllUserAddressesAsync(string userId);
    }
}
