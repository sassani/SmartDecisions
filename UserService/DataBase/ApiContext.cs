using Microsoft.EntityFrameworkCore;
using UserService.Core.Domain;
using UserService.DataBase.Configurations;

namespace UserService.DataBase
{
	public class ApiContext : DbContext
    {
		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options)
		{
			
		}

		public virtual DbSet<User> User { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserDbConfig());
		}
	}
}
