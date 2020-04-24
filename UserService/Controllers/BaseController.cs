using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Controllers;
using DecissionCore.Core.DAL;
using DecissionCore.Core.Services.Interfaces;
using DecissionCore.DataBase;

namespace DecissionCore.Controllers
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