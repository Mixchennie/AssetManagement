using System.Collections;
using AssetManagementTest.DataObject;
using Newtonsoft.Json.Linq;

namespace AssetManagementTest.Model
{
    public static class EditUserDataModel
    {
        public static IEnumerable GetEditUserWithValidData()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "EditUserData.json");
            var jsonData = File.ReadAllText(jsonPath);
            var jobject = JObject.Parse(jsonData);
            var validList = jobject["EditUserWithValidData"].ToObject<List<EditUserDataDto>>();

            foreach (var data in validList)
            {
                yield return new TestCaseData(data).SetName($"EditUserWithValidDataTest_{data.LastName}_{data.FirstName}");
            }
        }
    }
}
