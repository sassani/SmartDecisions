//using Castle.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace RestApi.Core.Security
{
	public class AppRSA
	{
		public IConfiguration Configuration { get; }
		public AppRSA(IConfiguration configuration)
		{
			Configuration = configuration;
		}



	}
}
