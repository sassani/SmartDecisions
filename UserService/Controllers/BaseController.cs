using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Controllers;
using UserService.Core.DAL;
using UserService.Core.Services.Interfaces;
using UserService.DataBase;

namespace UserService.Controllers
{
    [ApiController]
    public class BaseController : ServicesBaseController
    {
        protected IUnitOfWork UnitOfWork;
        public BaseController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            ServiceCode = GLOBAL_CONSTANTS.SERVICES.USER_SERVICE.CODE;
            Controllers = GLOBAL_CONSTANTS.SERVICES.USER_SERVICE.CONTROLLERS;
        }
    }
}