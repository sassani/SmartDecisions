using System;
using System.Threading.Tasks;
using ApplicationService.Core.DAL.IRepositories;

namespace ApplicationService.Core.DAL
{
    public interface IUnitOfWork: IDisposable
    {
        IProfileRepo Profile { get; }
        IContactRepo Contact { get; }
        IAvatarRepo Avatar { get; }


        Task<int> Complete();
    }
}
