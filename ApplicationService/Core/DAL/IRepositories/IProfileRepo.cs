using System.Threading.Tasks;
using ApplicationService.Core.Domain;
using Shared.DAL.Interfaces;

namespace ApplicationService.Core.DAL.IRepositories
{
    public interface IProfileRepo : IRepository<Profile>
    {
        Task<Profile> GetWithAddressAsync(string id);
    }
}
