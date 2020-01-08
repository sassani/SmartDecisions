﻿using RestApi.Core.Domain;
using RestApi.DataBase.Configurations;
using Microsoft.EntityFrameworkCore;
using System;

namespace RestApi.DataBase.Persistence
{
    public partial class ApiContext
    {
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Credential> Credential { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Logsheet> Logsheet { get; set; }
        public virtual DbSet<CredentialRole> CredentialRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientDbConfig(config));
            modelBuilder.ApplyConfiguration(new CredentialDbConfig(config));
            modelBuilder.ApplyConfiguration(new RoleDbConfig());
            modelBuilder.ApplyConfiguration(new CredentialRoleDbConfig());
        }
    }
}