using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Response;
using DecissionService.Core.DAL;
using DecissionService.Core.Domain;
using DecissionService.Core.Domain.DTOs;

namespace DecissionService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelAttributeFilter))]
    [Authorize]
    public class UserController : BaseController
    {
        private IMapper mapper;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            this.mapper = mapper;
        }


        [EndPointData("00")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            User user = await UnitOfWork.User.GetWithAddressAsync(GetCredentialId());
            UserDto userDto = mapper.Map<UserDto>(user);
            return new Response(HttpStatusCode.Accepted, userDto).ToActionResult();
        }

        [EndPointData("01")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserDto userDto)
        {
            try
            {
                string userId = GetCredentialId();
                if (UnitOfWork.User.Get(userId) != null) throw new Exception($"A User with Id ({userId}) is already registered");

                User newUser = mapper.Map<User>(userDto);
                newUser.Id = userId;

                UnitOfWork.User.Add(newUser);
                await UnitOfWork.Complete();
                userDto = mapper.Map<UserDto>(newUser);
                return new Response(HttpStatusCode.Created, userDto).ToActionResult();
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.BadRequest,
                                               new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid User",
                            Detail = err.Message
                                            }}).ToActionResult();
            }

        }

        [EndPointData("02")]
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UserDto userDto)
        {
            try
            {
                string userId = GetCredentialId();
                User user = await UnitOfWork.User.GetWithAddressAsync(userId);
                if (user == null) throw new Exception($"A User with Id ({userId}) Is not registered yet");

                mapper.Map(userDto, user);

                await UnitOfWork.Complete();
                //userDto = mapper.Map<UserDto>(user);
                var payload = new
                {
                    updatedUser = mapper.Map<UserDto>(user)
                };

                return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.BadRequest,
                                               new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid User",
                            Detail = err.Message
                                            }}).ToActionResult();
            }

        }

    }
}
