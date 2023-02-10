using ApplicationService.Core;
using ApplicationService.Core.Domain;
using ApplicationService.DataBase.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.DataBase
{
	public class ApiContext : DbContext
    {
		private readonly string userId;
		public ApiContext(DbContextOptions<ApiContext> options,
			IGetClaimsProvider userData)
			: base(options)
		{
			userId = userData.UserId;
		}

		public virtual DbSet<Profile> Profile { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new ProfileDbConfig());

			//modelBuilder.Entity<Address>().HasQueryFilter(a => a.OwnerId == userId);
		}
	}
}
