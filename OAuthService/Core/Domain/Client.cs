﻿using RestApi.Core.DataServices;
using RestApi.Helpers;
using Microsoft.AspNetCore.Http;
using UAParser;
using System.Collections.Generic;

namespace RestApi.Core.Domain
{
	public class Client
	{
        public Client()
        {
            Logsheet = new HashSet<Logsheet>();
        }

        public int Id { get; set; }
		public string ClientPublicId { get; set; }
		public string ClientSecret { get; set; }
		public string Name { get; set; }
		public AppEnums.ClientType Type { get; set; }

		public bool  IsValid { get; set; }
		public string Platform { get; set; }
		public string Browser { get; set; }
		public string IP { get; set; }


        public virtual ICollection<Logsheet> Logsheet { get; set; }
	}
}