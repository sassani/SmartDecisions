#nullable disable
using System.Collections.Generic;
using ApplicationService.Core;

namespace ApplicationService.Extensions
{
    public class AppSettingsModel
    {
        public string DbConnectionString { get; set; }
        public string DatabaseProvider { get; set; }
        public string SharedApiKey { get; set; }
        public string BaseUrl { get; set; }
        public string[] CrossUrls { get; set; }
        public Token Token { get; set; }
        public string StorageRootDirectory { get; set; }
        public AllowedFileFormats AllowedFileFormats { get; set; }
        public AzureStorageManagement AzureStorageManagement { get; set; }
    }

    public class Token
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationMin { get; set; }
    }

    public class AllowedFileFormats
    {
        public List<string> Document { get; set; }
        public List<string> Image { get; set; }
        public List<string> Multimedia { get; set; }
    }

    public class AzureStorageManagement
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}