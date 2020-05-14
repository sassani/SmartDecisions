using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationService.Core.Services.Interfaces;
using ApplicationService.Extensions;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Storage;

namespace ApplicationService.Core.Services
{
    public class CloudFileService : IFileService
    {
        private readonly IOptions<AppSettingsModel> config;
        private readonly IStorageService storageService;

        public CloudFileService( IOptions<AppSettingsModel> config, IStorageService storageService)
        {
            this.config = config;
            this.storageService = storageService;
            this.storageService.CreateRootReference(
                config.Value.AzureStorageManagement.ConnectionString, 
                config.Value.AzureStorageManagement.ContainerName);
        }

        public bool IsAllowedFormat(IFormFile formFile, out string validFormats, AppEnums.FileFormats expectedFormat = AppEnums.FileFormats.All)
        {
            validFormats = "";
            AllowedFileFormats aff = config.Value.AllowedFileFormats;
            string currentExt = Path.GetExtension(formFile.FileName);
            List<string> permittedExt = new List<string>();
            switch (expectedFormat)
            {
                case AppEnums.FileFormats.Document:
                    permittedExt = aff.Document;
                    break;
                case AppEnums.FileFormats.Image:
                    permittedExt = aff.Image;
                    break;
                case AppEnums.FileFormats.Multimedia:
                    permittedExt = aff.Multimedia;
                    break;
                default:
                    {
                        permittedExt.AddRange(aff.Document);
                        permittedExt.AddRange(aff.Image);
                        permittedExt.AddRange(aff.Multimedia);
                    }
                    break;
            }

            foreach (string item in permittedExt)
            {
                if (currentExt.ToLower() == item) return true;
            }
            validFormats = String.Join(", ", permittedExt);
            return false;

            //NOTE: check file header signature as well (do not trust user!)
        }

        public async Task<bool> RemoveAsync(string uri)
        {
            return await storageService.RemoveAsync(uri);
        }

        public async Task<List<string>> SaveAsync(List<IFormFile> formFiles, string path)
        {
            List<string> pathes = new List<string>();
            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    var uri = await storageService.UploadAsync(formFile, $"{path}/{StringHelper.GenerateRandom(18)}{Path.GetExtension(formFile.FileName)}");
                    pathes.Add(uri);
                }
            }
            return pathes;
        }

        public async Task<string> SaveAsync(IFormFile file, string path)
        {
            var target = await SaveAsync(new List<IFormFile> { file }, path);
            return target.Count != 0 ? target[0] : "";
        }
    }
}
