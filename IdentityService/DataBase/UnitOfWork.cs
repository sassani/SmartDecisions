using IdentityService.Core.DAL.IRepositories;
using IdentityService.Core.DAL;
using IdentityService.DataBase.Persistence;
using IdentityService.DataBase.Persistence.Repositories;
using System.Threading.Tasks;

namespace IdentityService.DataBase
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

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
