﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OAuthService.Core.Domain;
using Shared.DAL.Configurations;

namespace OAuthService.DataBase.Configurations
{
    public class CredentialRoleDbConfig : EntityConfiguration<CredentialRole>
    {
        public override void Config(EntityTypeBuilder<CredentialRole> builder)
        {

        }

        public override void Seed(EntityTypeBuilder<CredentialRole> builder)
        {
            builder.HasData(new CredentialRole
            {
                Id = 1,
                CredentialId = 1,
                RoleId = 1
            });

        }
    }
}
