using Microsoft.Extensions.Configuration;

namespace AssetManagementTest.Utils
{
    public static class ConfigurationUtils
    {
        private static IConfiguration _config;

        public static IConfiguration ReadConfiguration(string path)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path)
                .Build();

            _config = config;
            return config;
        }

        public static string GetConfigurationByKey(string key, IConfiguration? config = null)
        {
            var value = config == null ? _config[key] : config[key];

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            throw new InvalidDataException($"Attribute [{key}] is not been set in appsettings.json");
        }
    }
}
