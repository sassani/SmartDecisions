using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApplicationService.Core.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<string>> SaveAsync(List<IFormFile> files, string path);
        Task<string> SaveAsync(IFormFile file, string path);
        Task<bool> RemoveAsync(string fileFullPath);
        bool IsAllowedFormat(IFormFile formFile, out string validFormats, AppEnums.FileFormats expectedFormat=0);
    }
}
