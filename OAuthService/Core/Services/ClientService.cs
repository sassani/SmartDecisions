using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Helpers;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Core.DataServices;

using Microsoft.AspNetCore.Http;
using System;
using UAParser;

namespace OAuthService.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClientService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Client CreateClient(string clientId, string clientSecret=null)
        {
            Client client = new Client
            {
                ClientPublicId = clientId,
                ClientSecret = clientSecret,
            };
            var t = CheckValidation(ref client);
            client.IsValid = t;
            return client;
        }

        private bool CheckValidation(ref Client client)
        {
            client = unitOfWork.Client.FindByClientPublicId(client.ClientPublicId);
            //Client clientDb = unitOfWork.Client.FindByClientPublicId(client.ClientPublicId);
            if (client == null) return false;

            if (client.Type.Equals(AppEnums.ClientType.Mobile))
            {
                if (!client.ClientSecret.Equals(client.ClientSecret)) return false;
            }

            //Mapper.MapDbModelToClassModel(client, clientDb);
            //client = clientDb;
            ClientParser(client);
            return true;
        }

        private void ClientParser(Client client)
        {
            /// ref:https://github.com/ua-parser/uap-csharp
            client.IP = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string uaString = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();

            // get a parser with the embedded regex patterns
            var uaParser = Parser.GetDefault();

            // get a parser using externally supplied yaml definitions
            // var uaParser = Parser.FromYaml(yamlString);

            ClientInfo clientInfo = uaParser.Parse(uaString);

            client.Browser = clientInfo.UA.Family + " " + clientInfo.UA.Major + "." + clientInfo.UA.Minor;
            if (client.Browser == "Other .")
            {
                client.Browser = uaString;
            }

            if (clientInfo.Device.Family != "Other")
                client.Platform = clientInfo.Device.Family + " " + clientInfo.Device.Model + " ";

            client.Platform += clientInfo.OS.Family;

            if (clientInfo.OS.Major != null)
            {
                client.Platform += " " + clientInfo.OS.Major + "." + clientInfo.OS.Minor;
            }

        }
    }
}
