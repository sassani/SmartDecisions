using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IdentityService.Extensions;

namespace IdentityService.DataBase.Persistence
{
	public partial class ApiContext : DbContext
	{
		private readonly IOptions<AppSettingsModel> config;
		public ApiContext(DbContextOptions<ApiContext> options, IOptions<AppSettingsModel> config)
			: base(options)
		{
			this.config = config;
		}






	}
}