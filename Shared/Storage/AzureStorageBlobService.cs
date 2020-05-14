using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Shared.Storage
{
    public class AzureStorageBlobService : IStorageService
    {
        private BlobContainerClient container;

        public void CreateRootReference(string accessKey, string rootDirectory)
        {
            container = new BlobContainerClient(accessKey, rootDirectory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>a list of file names in the target</returns>
        public async Task<IEnumerable<string>> GetFilesAsync(string path = null)
        {
            Azure.AsyncPageable<BlobItem> blobs;
            List<string> result = new List<string>();
            if (path == null)
            {
                blobs = container.GetBlobsAsync();
            }
            else
            {
                blobs = container.GetBlobsAsync(prefix: path + "/");
            }

            await foreach (var blob in blobs)
            {
                result.Add(blob.Name);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="blobName"></param>
        /// <returns>Uploaded file URI</returns>
        public async Task<string> UploadAsync(IFormFile file, string blobName)
        {
            BlobClient blobClient = container.GetBlobClient(blobName);
            var h = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };
            await blobClient.UploadAsync(file.OpenReadStream(), h);
            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files_names"></param>
        /// <returns>list of uploaded files URIs</returns>
        public async Task<IEnumerable<string>> UploadAsync(IDictionary<IFormFile, string> files_names)
        {
            List<string> result = new List<string>();
            foreach (var file_name in files_names)
            {
                BlobClient blobClient = container.GetBlobClient(file_name.Value);

                var h = new BlobHttpHeaders
                {
                    ContentType = file_name.Key.ContentType
                };
                await blobClient.UploadAsync(file_name.Key.OpenReadStream(), h);
                result.Add(blobClient.Uri.ToString());
            }
            return result;
        }

        public async Task<bool> RemoveAsync(string uriString)
        {
            BlobClient blobClient = container.GetBlobClient(new BlobUriBuilder(new Uri(uriString)).BlobName);
            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
