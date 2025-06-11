using System.Collections;
using AssetManagementTest.DataObject;
using Newtonsoft.Json.Linq;

namespace AssetManagementTest.Model
{
    public static class CreateUserDataModel
    {
        public static IEnumerable GetAdminUserData()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "CreateUserData.json");
            var jsonData = File.ReadAllText(jsonPath);
            var jobject = JObject.Parse(jsonData);
            var validList = jobject["CreateAdminUser"].ToObject<List<CreateUserDataDto>>();

            foreach (var data in validList)
            {
                yield return new TestCaseData(data).SetName($"CreateAdminUserTest_{data.LastName}_{data.FirstName}_{data.Email}");
            }
        }

        public static IEnumerable GetStaffUserData()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "CreateUserData.json");
            var jsonData = File.ReadAllText(jsonPath);
            var jobject = JObject.Parse(jsonData);
            var validList = jobject["CreateStaffUser"].ToObject<List<CreateUserDataDto>>();

            foreach (var data in validList)
            {
                yield return new TestCaseData(data).SetName($"CreateStaffUserTest_{data.LastName}_{data.FirstName}_{data.Email}");
            }
        }
    }
}
