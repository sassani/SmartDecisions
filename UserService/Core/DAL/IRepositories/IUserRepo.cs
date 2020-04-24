using System.Threading.Tasks;
using Shared.DAL.Interfaces;
using DecissionService.Core.Domain;

namespace DecissionService.Core.DAL.IRepositories
{
    public interface IUserRepo : IRepository<User>
    {
        Task<User> GetWithAddressAsync(string id);
    }
}
