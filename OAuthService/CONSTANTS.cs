using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService
{
    public static class CONSTANTS
    {
        public static class REQUEST_TYPE
        {
            public const string REFRESH_TOKEN = "refreshtoken";
            public const string ID_TOKEN = "idtoken";
            public const string FORGOT_PASSWORD = "forgotpassword";
            public const string CHANGE_PASSWORD = "changepassword";
            public const string REGISTER = "register";
        }
    }
}
