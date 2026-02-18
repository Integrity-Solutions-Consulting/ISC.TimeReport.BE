using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace isc.time.report.be.infrastructure.Utils.Secutiry
{
    public class CryptoHelper
    {

        private readonly string _key;
        public CryptoHelper(IConfiguration config)
        {
            _key = config["Security:EncryptionKey"]
                ?? throw new Exception("EncryptionKey no configurada");
        }

        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_key.PadRight(32));
            aes.IV = new byte[16];

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(fullCipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
