using System;
using System.Threading.Tasks;
using DecissionService.Core.DAL.IRepositories;

namespace DecissionService.Core.DAL
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepo User { get; }
        IAddressRepo Address { get; }


        Task<int> Complete();
    }
}
