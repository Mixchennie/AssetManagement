using System.Collections;
using AssetManagementTest.DataObject;
using Newtonsoft.Json.Linq;

namespace AssetManagementTest.Model
{
    public static class CreateAssignmentDataModel
    {
        public static IEnumerable GetValidAssignmentData()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "CreateAssignmentData.json");
            var jsonData = File.ReadAllText(jsonPath);
            var jobject = JObject.Parse(jsonData);
            var validList = jobject["CreateAssignment"].ToObject<List<CreateAssignmentDataDto>>();

            foreach (var data in validList)
            {
                yield return new TestCaseData(data).SetName($"CreateAssignment_{data.UserFullName}_{data.AssetName}_{data.AssignedDate}");
            }
        }
    }
}