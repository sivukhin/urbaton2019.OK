using System.IO;

namespace CleanCityCore.Infra
{
    public class SecretManager
    {
        public string GetSecret(string secretKey)
        {
            return File.ReadAllText($"secret_{secretKey}.config").Trim();
        }
    }
}