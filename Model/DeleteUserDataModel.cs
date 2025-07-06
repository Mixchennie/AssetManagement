using System.Collections;
using AssetManagementTest.DataObject;
using Newtonsoft.Json.Linq;

namespace AssetManagementTest.Model
{
    public static class DeleteUserDataModel
    {
        public static IEnumerable GetDeleteUserData()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "DeleteUserData.json");
            var jsonData = File.ReadAllText(jsonPath);
            var jobject = JObject.Parse(jsonData);
            var validList = jobject["DeleteUserData"].ToObject<List<DeleteUserDataDto>>();

            foreach (var data in validList)
            {
                yield return new TestCaseData(data).SetName($"DeleteUserDataTest_{data.LastName}_{data.FirstName}");
            }
        }
    }
}
