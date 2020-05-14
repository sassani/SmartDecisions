using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared.Storage
{
    public interface IStorageService
    {
        Task<string> UploadAsync(IFormFile file, string blobName);
        Task<IEnumerable<string>> UploadAsync(IDictionary<IFormFile, string> files_names);
        Task<bool> RemoveAsync(string fileFullPath);
        Task<IEnumerable<string>> GetFilesAsync(string path = null);
        void CreateRootReference(string accessKey, string rootDirectory);
    }
}
