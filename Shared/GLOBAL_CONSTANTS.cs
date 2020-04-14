using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public static class GLOBAL_CONSTANTS
    {
        public static class SERVICES
        {
            public static class AUTH_SERVICE
            {
                public const string NAME = "OAuthService";
                public const string CODE = "01";
                public static readonly Dictionary<string, string> CONTROLLERS = new Dictionary<string, string>() {
                    { "Info","00" },
                    { "Auth","01" },
                    { "Credential","02" },
                };
            }
            public static class USER_SERVICE
            {
                public const string NAME = "UserService";
                public const string CODE = "02";
                public static readonly Dictionary<string, string> CONTROLLERS = new Dictionary<string, string>() {
                    { "Info","00" },
                    { "User","01" },
                };
            }
        }
    }
}
