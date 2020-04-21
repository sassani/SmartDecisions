using System.Threading.Tasks;
using Shared.DAL.Interfaces;
using UserService.Core.Domain;

namespace UserService.Core.DAL.IRepositories
{
    public interface IUserRepo : IRepository<User>
    {
        Task<User> GetWithAddressAsync(string id);
    }
}
