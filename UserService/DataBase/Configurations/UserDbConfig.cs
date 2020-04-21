using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.DAL.Configurations;
using UserService.Core.Domain;

namespace UserService.DataBase.Configurations
{
    public class UserDbConfig : EntityConfiguration<User>
    {
        public UserDbConfig()
        {
        }

        public override void Config(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).HasColumnType("VARCHAR(36)");
        }
    }
}
