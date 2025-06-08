using Newtonsoft.Json;

namespace AssetManagementTest.Helper
{
    public static class JsonDataReader
    {
        public static T ReadDataFromJson<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found at path: {filePath}");

            var jsonContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonContent)!;
        }
    }
}