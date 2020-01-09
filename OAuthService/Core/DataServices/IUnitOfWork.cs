using System;
using OAuthService.Core.DataServices.IRepositories;

namespace OAuthService.Core.DataServices
{
	public interface IUnitOfWork : IDisposable
	{
		IClientRepo Client { get; }
		ICredentialRepo Credential { get; }
		ILogsheetRepo Logsheet { get; }

		int Complete();
	}
}
