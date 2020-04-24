using System.Threading.Tasks;
using Shared.DAL.Interfaces;
using DecissionCore.Core.Domain;

namespace DecissionCore.Core.DAL.IRepositories
{
    public interface IUserRepo : IRepository<User>
    {
        Task<User> GetWithAddressAsync(string id);
    }
}
