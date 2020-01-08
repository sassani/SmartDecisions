using RestApi.Core.DataServices.IRepositories;
using RestApi.Core.DataServices;
using RestApi.DataBase.Persistence;
using RestApi.DataBase.Persistence.Repositories;

namespace RestApi.DataBase
{
	/// <summary>
	/// All needed Database transactions mus be handled by this class
	/// </summary>
	/// <remarks>
	/// Add more details here.
	/// </remarks>
	public sealed class UnitOfWork : IUnitOfWork
	{
		private readonly ApiContext context;

		public ICredentialRepo Credential { get; }
		public IClientRepo Client { get; }
		public ILogsheetRepo Logsheet { get; set; }

		public UnitOfWork(ApiContext context)
		{
			this.context = context;
			Credential = new CredentialRepo(context);
			Client = new ClientRepo(context);
			Logsheet = new LogsheetRepo(context);
		}

		public int Complete()
		{
			return context.SaveChanges();
		}

		public void Dispose()
		{
			context.Dispose();
		}
	}
}
