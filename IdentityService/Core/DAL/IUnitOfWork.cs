using System;
using System.Threading.Tasks;
using IdentityService.Core.DAL.IRepositories;

namespace IdentityService.Core.DAL
{
	public interface IUnitOfWork : IDisposable
	{
		IClientRepo Client { get; }
		ICredentialRepo Credential { get; }
		ILogsheetRepo Logsheet { get; }

		int Complete();
        Task<int> CompleteAsync();
    }
}
