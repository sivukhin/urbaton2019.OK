using System.IO;

namespace CleanCityCore.Infra
{
    public class SecretManager
    {
        public string GetNonSecret(string key)
        {
            return File.ReadAllText($"non_secret_{key}.config").Trim();
        }

        public string GetSecret(string secretKey)
        {
            return File.ReadAllText($"secret_{secretKey}.config").Trim();
        }
    }
}