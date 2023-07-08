using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityService.Core.Security
{
    public static class TokenRSA
    {
        private static string CERT_PATH = $"{Environment.GetEnvironmentVariable("ASPNET_APPSETTINGS_SSS_IDENTITY")}/cert";
        public static async Task<RSA> RsaPrivateKeyAsync()
        {
            var privateKey = RSA.Create();
            /* Use openssl tool to create RSA private and public keys:
               openssl genpkey -algorithm RSA -pkeyopt rsa_keygen_bits:2048 -out rsa.der -outform DER
               openssl pkey -inform DER -in rsa.der -pubout -out rsa_pub.pem
             */
            byte[] privateKeyBytes = await File.ReadAllBytesAsync($"{CERT_PATH}/rsa.der");

            privateKey.ImportRSAPrivateKey(privateKeyBytes, out _);
            return privateKey;
        }
        public static async Task<RSA> RsaPublicKey()
        {
            var publicKey = RSA.Create();
            publicKey.ImportFromPem(await PublicKeyString());
            return publicKey;
        }

        public static async  Task<string>PublicKeyString()
        {
            string publicKeyString;
            using (var file = File.OpenText($"{CERT_PATH}/rsa_pub.pem"))
            {
                publicKeyString = await file.ReadToEndAsync();
            }
            return publicKeyString;
        }
    }
}
