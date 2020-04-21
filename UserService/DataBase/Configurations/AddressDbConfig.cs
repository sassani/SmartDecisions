﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.DAL.Configurations;
using UserService.Core.Domain;

namespace UserService.DataBase.Configurations
{
    public class AddressDbConfig : EntityConfiguration<Address>
    {
        public override void Config(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.UserId).HasColumnType("varchar(36)");
            builder.HasOne(a => a.User).WithMany(u => u.Addresses).HasForeignKey(a => a.UserId);
        }
    }
}
