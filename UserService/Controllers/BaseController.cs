using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Controllers;
using DecissionService.Core.DAL;
using DecissionService.Core.Services.Interfaces;
using DecissionService.DataBase;

namespace DecissionService.Controllers
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