using System;
using System.Threading.Tasks;
using UserService.Core.DAL.IRepositories;

namespace UserService.Core.DAL
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepo User { get; }
        IAddressRepo Address { get; }


        Task<int> Complete();
    }
}
