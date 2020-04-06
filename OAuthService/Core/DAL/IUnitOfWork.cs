using System;
using OAuthService.Core.DAL.IRepositories;

namespace OAuthService.Core.DAL
{
	public interface IUnitOfWork : IDisposable
	{
		IClientRepo Client { get; }
		ICredentialRepo Credential { get; }
		ILogsheetRepo Logsheet { get; }

		int Complete();
	}
}
