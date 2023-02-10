using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityService.Core.Domain;
using Shared.DAL.Configurations;
using static IdentityService.Core.AppEnums;

namespace IdentityService.DataBase.Configurations
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
                    Id = (int)role!,
                    Type = (RoleType)role,
                });
            }
        }
    }
}
