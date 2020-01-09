using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OAuthService.Core.Domain;
using static OAuthService.Core.AppEnums;
using System;

namespace OAuthService.DataBase.Configurations
{
    public class RoleDbConfig : EntityConfiguration<Role>
    {
        public override void Config(EntityTypeBuilder<Role> builder)
        {

        }

        public override void Seed(EntityTypeBuilder<Role> builder)
        {
            foreach (var role in Enum.GetValues(typeof(RoleType)))
            {
                builder.HasData(new Role
                {
                    Id = (int)role,
                    Type = (RoleType)role,
                });
            }
        }
    }
}
