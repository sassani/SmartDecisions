using ApplicationService.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.DAL.Configurations;

namespace ApplicationService.DataBase.Configurations
{
    public class ProfileDbConfig : EntityConfiguration<Profile>
    {
        public ProfileDbConfig()
        {
        }

        public override void Config(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p => p.OwnerId);
            builder.HasIndex(p => p.OwnerId).IsUnique();
        }
    }
}
