using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApplicationService.Core.Services.Interfaces;
using ApplicationService.Extensions;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ApplicationService.Core.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IOptions<AppSettingsModel> config;

        public LocalFileService(IOptions<AppSettingsModel> config)
        {
            this.config = config;
        }

        public async Task<bool> RemoveAsync(string fileFullPath)
        {
            try
            {
                await Task.Run(() => { File.Delete(fileFullPath); });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAllowedFormat(IFormFile formFile, out string validFormats, AppEnums.FileFormats expectedFormat = 0)
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

        public async Task<List<string>> SaveAsync(List<IFormFile> files, string path)
        {
            List<string> pathes = new List<string>();
            var filePath = Path.Combine(config.Value.StorageRootDirectory, path);
            Directory.CreateDirectory(filePath);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileNme = $"{StringHelper.GenerateRandom(18)}{Path.GetExtension(formFile.FileName)}";
                    string fullFilePath = Path.Combine(filePath, fileNme);

                    using (var stream = File.Create(fullFilePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    pathes.Add(fullFilePath);
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
