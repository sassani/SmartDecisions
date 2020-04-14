using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Controllers;

namespace UserService.Controllers
{
    [ApiController]
    public class BaseController : ServicesBaseController
    {
        public BaseController()
        {
            ServiceCode = GLOBAL_CONSTANTS.SERVICES.USER_SERVICE.CODE;
            Controllers = GLOBAL_CONSTANTS.SERVICES.USER_SERVICE.CONTROLLERS;
        }
    }
}