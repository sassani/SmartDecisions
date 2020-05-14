using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public static class GLOBAL_CONSTANTS
    {
        public static class SERVICES
        {
            public static class IDENTITY_SERVICE
            {
                public const string NAME = "IdentityService";
                public const string CODE = "01";
                public static readonly Dictionary<string, string> CONTROLLERS = new Dictionary<string, string>() {
                    { "Info","00" },
                    { "Auth","01" },
                    { "Credential","02" },
                };
            }
            public static class APPLICATION_SERVICE
            {
                public const string NAME = "ApplicationService";
                public const string CODE = "02";
                public static readonly Dictionary<string, string> CONTROLLERS = new Dictionary<string, string>() {
                    { "Info","00" },
                    { "Profile","01" }
                };
            }
        }
    }
}
