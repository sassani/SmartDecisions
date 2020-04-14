using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Core.Domain;

namespace UserService.DataBase.Configurations
{
    public class UserDbConfig : EntityConfiguration<User>
    {
        public override void Config(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.PublicId);
        }
    }
}
