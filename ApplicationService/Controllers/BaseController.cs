using ApplicationService.Core.DAL;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Controllers;

namespace ApplicationService.Controllers
{
    [ApiController]
    public class BaseController : ServicesBaseController
    {
        protected IUnitOfWork UnitOfWork;
        public BaseController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            ServiceCode = GLOBAL_CONSTANTS.SERVICES.APPLICATION_SERVICE.CODE;
            Controllers = GLOBAL_CONSTANTS.SERVICES.APPLICATION_SERVICE.CONTROLLERS;
        }
    }
}