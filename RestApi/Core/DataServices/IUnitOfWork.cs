using System;
using RestApi.Core.DataServices.IRepositories;

namespace RestApi.Core.DataServices
{
	public interface IUnitOfWork : IDisposable
	{
		IClientRepo Client { get; }
		ICredentialRepo Credential { get; }
		ILogsheetRepo Logsheet { get; }

		int Complete();
	}
}
