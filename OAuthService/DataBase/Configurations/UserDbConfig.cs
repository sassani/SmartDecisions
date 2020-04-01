using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OAuthService.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.DataBase.Configurations
{
    public class UserDbConfig : EntityConfiguration<User>
    {
        public override void Config(EntityTypeBuilder<User> builder)
        {
            

        }
    }
}
