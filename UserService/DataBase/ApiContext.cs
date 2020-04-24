using Microsoft.EntityFrameworkCore;
using DecissionService.Core;
using DecissionService.Core.Domain;
using DecissionService.DataBase.Configurations;

namespace DecissionService.DataBase
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

		public virtual DbSet<User> User { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserDbConfig());

			//modelBuilder.Entity<Address>().HasQueryFilter(a => a.UserId == userId);
		}
	}
}
