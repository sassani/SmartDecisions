using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Filters;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Response;
using UserService.Core.DAL;
using UserService.Core.Domain;
using UserService.Core.Domain.DTOs;

namespace UserService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelAttributeFilter))]
    public class UserController : BaseController
    {
        private IMapper mapper;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            this.mapper = mapper;
        }


        [EndPointData("00")]
        [HttpGet]
        public IActionResult Get()
        {
            var payload = new
            {
                user = GetCredentialId()

            };
            return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
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
                List<string> list = new List<string>();

                foreach (var item in userDto.GetType().GetProperties())
                {
                    var itemValue = userDto.GetType().GetProperty(item.Name)!.GetValue(userDto);
                    if (itemValue != null && item.PropertyType.Namespace != "System.Collections.Generic")
                    {
                        user.GetType().GetProperty(item.Name)!.SetValue(user, itemValue);
                    }
                }

                await UnitOfWork.Complete();
                userDto = mapper.Map<UserDto>(user);

                return new Response(HttpStatusCode.Accepted, userDto).ToActionResult();
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
