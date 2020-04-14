using Shared.DAL.Interfaces;
using UserService.Core.Domain;

namespace UserService.Core.DAL.IRepositories
{
    interface IUserRepo : IRepository<User>
    {
    }
}
