using OAuthService.Core.Domain;
using OAuthService.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace OAuthService.DataBase.Persistence
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