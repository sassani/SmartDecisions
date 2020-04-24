using System;
using System.Collections.Generic;
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
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelAttributeFilter))]
    public class AddressController : BaseController
    {
        private IMapper mapper;
        public AddressController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            this.mapper = mapper;
        }

        [EndPointData("00")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get()
        {
            var addresses = await UnitOfWork.Address.GetAllUserAddressesAsync(GetCredentialId());
            return new Response(HttpStatusCode.OK, mapper.Map<List<AddressDto>>(addresses)).ToActionResult();
        }

        [EndPointData("01")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await UnitOfWork.Address.GetAllUserAddressesAsync(GetCredentialId());
            return new Response(HttpStatusCode.OK, mapper.Map<List<AddressDto>>(addresses)).ToActionResult();
        }

        [EndPointData("02")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddressDto addressDto)
        {
            try
            {
                Address newAddress = mapper.Map<Address>(addressDto);
                newAddress.UserId = GetCredentialId();

                UnitOfWork.Address.Add(newAddress);
                await UnitOfWork.Complete();
                var addresses = await UnitOfWork.Address.GetAllUserAddressesAsync(GetCredentialId());
                return new Response(HttpStatusCode.Accepted, mapper.Map<List<AddressDto>>(addresses)).ToActionResult();
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.BadRequest,
                                               new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid Address",
                            Detail = err.InnerException !=null? err.InnerException.Message : err.Message
                                            }}).ToActionResult();
            }
        }

        [EndPointData("03")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] AddressDto addressDto)
        {
            try
            {
                Address address = UnitOfWork.Address.Get(id);
                mapper.Map(addressDto, address);
                await UnitOfWork.Complete();
                return new Response(HttpStatusCode.Accepted, mapper.Map<AddressDto>(address)).ToActionResult();
            }
            catch (Exception err)
            {
                return new Response(HttpStatusCode.BadRequest,
                                                              new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid Address",
                            Detail = err.InnerException !=null? err.InnerException.Message : err.Message
                                            }}).ToActionResult();
            }
        }

        [EndPointData("04")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                Address address = UnitOfWork.Address.Get(id);
                UnitOfWork.Address.Remove(address);
                await UnitOfWork.Complete();
                return new Response(HttpStatusCode.NoContent).ToActionResult();
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.BadRequest,
                                               new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Removing Address Failed",
                            Detail = err.InnerException !=null? err.InnerException.Message : err.Message
                                            }}).ToActionResult();
            }
        }
    }
}
