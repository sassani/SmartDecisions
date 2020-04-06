using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Helpers;
using System;
using Microsoft.Extensions.Options;
using OAuthService.Extensions;
using OAuthService.Core.Domain;

namespace OAuthService.DataBase.Configurations
{
    public class CredentialDbConfig : EntityConfiguration<Credential>
    {
        private readonly IOptions<AppSettingsModel> config;

        public CredentialDbConfig(IOptions<AppSettingsModel> config)
        {
            this.config = config;
        }
        public override void Config(EntityTypeBuilder<Credential> builder)
        {
            builder.HasIndex(f => f.PublicId).IsUnique();
            builder.Property(f => f.PublicId).IsRequired();
            builder.Property(f => f.Email).IsUnicode().IsRequired().HasMaxLength(25);
            builder.Property(f => f.Password).IsRequired().HasMaxLength(75);
            builder.Property(f => f.IsActive).HasColumnType("TINYINT(1)");
            builder.Property(f => f.IsEmailVerified).HasColumnType("TINYINT(1)");

            //Ignored properties
            builder.Ignore(f => f.IsAuthenticated);
        }

        public override void Seed(EntityTypeBuilder<Credential> builder)
        {
            var admin = config.Value.BaseAdmin;
            builder.HasData(new Credential
            {
                Id = 1,
                PublicId = Guid.NewGuid().ToString(),
                Email = admin.Email,
                Password = StringHelper.StringToHash(admin.Password),
                IsActive = true,
                IsEmailVerified = true,
                LastLoginAt = null
            });
        }
    }
}
