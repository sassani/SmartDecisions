using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApi.Helpers;
using Microsoft.Extensions.Options;
using RestApi.Extensions;
using RestApi.Core.Domain;

namespace RestApi.DataBase.Configurations
{
    public class ClientDbConfig : EntityConfiguration<Client>
    {
        private readonly IOptions<AppSettingsModel> config;

        public ClientDbConfig(IOptions<AppSettingsModel> config)
        {
            this.config = config;
        }
        public override void Config(EntityTypeBuilder<Client> builder)
        {
            builder.Property(f => f.ClientPublicId).IsRequired().HasMaxLength(25);
            builder.Property(f => f.ClientSecret).IsRequired().HasMaxLength(75);

            //Ignored properties
            builder.Ignore(f => f.Browser);
            builder.Ignore(f => f.Platform);
            builder.Ignore(f => f.IP);
            builder.Ignore(f => f.IsValid);
        }

        public override void Seed(EntityTypeBuilder<Client> builder)
        {
            var client = config.Value.BaseClient;
            builder.HasData(new Client
            {
                Id = 1,
                ClientPublicId = client.ClientId,
                ClientSecret = StringHelper.StringToHash(client.ClientSecret),
                Name = client.Name,
                Type = client.Type
            });
        }
    }
}
