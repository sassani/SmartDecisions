using System;
using RestApi.DataBase.Persistence.Repositories.Interfaces;

namespace RestApi.DataBase.Core
{
	public interface IUnitOfWork : IDisposable
	{
		IClient Client { get; }
		IUser User { get; }
		ILogsheet Logsheet { get; }

		int Complete();
	}
}
