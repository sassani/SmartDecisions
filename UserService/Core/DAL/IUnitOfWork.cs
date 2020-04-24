using System;
using System.Threading.Tasks;
using DecissionCore.Core.DAL.IRepositories;

namespace DecissionCore.Core.DAL
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepo User { get; }
        IAddressRepo Address { get; }


        Task<int> Complete();
    }
}
