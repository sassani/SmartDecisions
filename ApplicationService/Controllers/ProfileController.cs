using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ApplicationService.Core;
using ApplicationService.Core.DAL;
using ApplicationService.Core.Domain;
using ApplicationService.Core.Domain.DTOs;
using ApplicationService.Core.Services.Interfaces;
using AutoMapper;
using Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Response;
using Shared.Responses;
using static ApplicationService.CONSTANTS;

namespace ApplicationService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelAttributeFilter))]
    [Authorize]
    public class ProfileController : BaseController
    {
        private IMapper mapper;
        private IFileService fileService;
        public ProfileController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService) : base(unitOfWork)
        {
            this.mapper = mapper;
            this.fileService = fileService;
        }


        #region Profile
        [EndPointData("00")]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            Core.Domain.Profile profile = await UnitOfWork.Profile.GetWithAddressAsync(base.GetCredentialId());
            ProfileDto profileDto = mapper.Map<ProfileDto>(profile);
            return new Response(HttpStatusCode.OK, profileDto).ToActionResult();
        }

        [EndPointData("01")]
        [HttpPost]
        public async Task<IActionResult> AddProfile([FromBody] ProfileDto profileDto)
        {
            try
            {
                string userId = GetCredentialId();
                if (UnitOfWork.Profile.Get(userId) != null) return Errors.Conflict(ErrorCode + "00");

                Core.Domain.Profile newProfile = mapper.Map<Core.Domain.Profile>(profileDto);
                newProfile.OwnerId = userId;

                UnitOfWork.Profile.Add(newProfile);
                await UnitOfWork.Complete();
                profileDto = mapper.Map<ProfileDto>(newProfile);
                return new Response(HttpStatusCode.Created, profileDto).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }

        }

        [EndPointData("02")]
        [HttpPatch]
        public async Task<IActionResult> EditProfile([FromBody] ProfileDto profileDto)
        {
            try
            {
                string userId = GetCredentialId();
                var profile = await UnitOfWork.Profile.GetWithAddressAsync(userId);
                if (profile == null) return Errors.NotFound(ErrorCode + "00", detail: $"A User with Id ({userId}) Is not registered yet");

                mapper.Map(profileDto, profile);

                await UnitOfWork.Complete();

                return new Response(HttpStatusCode.Accepted, mapper.Map<ProfileDto>(profile)).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }

        }

        [EndPointData("03")]
        [HttpDelete]
        public async Task<IActionResult> RemoveProfile()
        {
            try
            {
                string userId = GetCredentialId();
                var profile = await UnitOfWork.Profile.GetWithAddressAsync(userId);
                if (profile == null) return Errors.NotFound(ErrorCode + "00", detail: $"A User with Id ({userId}) Is not registered yet");
                UnitOfWork.Profile.Remove(profile);
                await UnitOfWork.Complete();
                return new Response(HttpStatusCode.Accepted).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }
        }
        #endregion

        #region Address
        [EndPointData("04")]
        [HttpGet("address/{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            try
            {
                var addresses = UnitOfWork.Address.Get(id);
                if (addresses != null && !addresses.IsOwnedByUser(GetCredentialId())) return Errors.Forbiden(ErrorCode + "00");
                return new Response(HttpStatusCode.OK, mapper.Map<AddressDto>(addresses)).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }
        }

        [EndPointData("05")]
        [HttpGet("address")]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await UnitOfWork.Address.GetAllUserAddressesAsync(GetCredentialId());
            return new Response(HttpStatusCode.OK, mapper.Map<List<AddressDto>>(addresses)).ToActionResult();
        }

        [EndPointData("06")]
        [HttpPost("address")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDto addressDto)
        {
            try
            {
                Address newAddress = mapper.Map<Address>(addressDto);
                newAddress.OwnerId = GetCredentialId();
                newAddress.ProfileId = newAddress.OwnerId;

                UnitOfWork.Address.Add(newAddress);
                await UnitOfWork.Complete();
                var addresses = await UnitOfWork.Address.GetAllUserAddressesAsync(GetCredentialId());
                return new Response(HttpStatusCode.Accepted, mapper.Map<List<AddressDto>>(addresses)).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }
        }

        [EndPointData("07")]
        [HttpPatch("address/{id}")]
        public async Task<IActionResult> EditAddress(int id, [FromBody] AddressDto addressDto)
        {
            try
            {
                Address address = UnitOfWork.Address.Get(id);
                if (address == null) return Errors.NotFound(ErrorCode + "00", detail: $"There is no address with id ({id})");
                if (!address.IsOwnedByUser(GetCredentialId())) return Errors.Forbiden(ErrorCode + "00");
                mapper.Map(addressDto, address);
                await UnitOfWork.Complete();
                return new Response(HttpStatusCode.Accepted, mapper.Map<AddressDto>(address)).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }
        }

        [EndPointData("08")]
        [HttpDelete("address/{id}")]
        public async Task<IActionResult> RemoveAddress(int id)
        {
            try
            {
                Address address = UnitOfWork.Address.Get(id);
                if (address == null) return Errors.NotFound(ErrorCode + "00", detail: $"There is no address with id ({id})");
                if (address.OwnerId != GetCredentialId()) Errors.Forbiden(ErrorCode + "00", detail: "you don't have permission to access this resource");
                UnitOfWork.Address.Remove(address);
                await UnitOfWork.Complete();
                return new Response(HttpStatusCode.NoContent).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }
        }
        #endregion

        #region Avatar
        [EndPointData("09")]
        [HttpPost("avatar")]
        public async Task<IActionResult> AddAvatar(IFormFile imageFile)
        {
            try
            {
                if (!fileService.IsAllowedFormat(imageFile, out string permitted, AppEnums.FileFormats.Image))
                    return Errors.BadRequest(ErrorCode + "03", "Invalid File Format", $"valid file formats: {permitted}");
                string userId = GetCredentialId();
                var profile = await UnitOfWork.Profile.GetWithAddressAsync(userId);
                if (profile == null) return Errors.NotFound(ErrorCode + "00", detail: $"A User with Id ({userId}) Is not registered yet");
                if (profile.Avatar != null) await fileService.RemoveAsync(profile.Avatar.Url);

                profile.Avatar = new Avatar()
                {
                    Url = await fileService.SaveAsync(imageFile, Path.Combine(STORAGE_PATH.AVATARAS, userId)),
                    OwnerId = profile.OwnerId,
                    ProfileId = profile.OwnerId
                };

                await UnitOfWork.Complete();


                return new Response(HttpStatusCode.Created, mapper.Map<ProfileDto>(profile)).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }

        }

        [EndPointData("10")]
        [HttpDelete("avatar/{id}")]
        public async Task<IActionResult> RemoveAvatar(int id)
        {
            string userId = GetCredentialId();
            var avatar = UnitOfWork.Avatar.Get(id);
            if (avatar == null) return Errors.NotFound(ErrorCode + "00", detail: $"There is no avatar with id ({id})");
            if (!avatar.IsOwnedByUser(userId)) return Errors.Forbiden(ErrorCode + "02");
            await fileService.RemoveAsync(avatar.Url);
            UnitOfWork.Avatar.Remove(avatar);
            await UnitOfWork.Complete();
            return new Response(HttpStatusCode.OK).ToActionResult();
        }
        #endregion

    }
}
