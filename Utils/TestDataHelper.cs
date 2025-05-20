using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeleniumPractice.TestData.DTO;

public static class TestDataHelper
{
    public static IEnumerable GetValidRegistrationFormTestData()
    {
        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "RegistrationFormData.json");
        var jsonData = File.ReadAllText(jsonPath);
        var jobject = JObject.Parse(jsonData);
        var validList = jobject["ValidRegistrations"].ToObject<List<RegistrationFormDTO>>();

        foreach (var data in validList)
        {
            yield return new TestCaseData(data).SetName($"ValidRegistration_{data.FirstName}_{data.LastName}");
        }
    }

    public static IEnumerable GetInvalidRegistrationFormTestData()
    {
        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "RegistrationFormData.json");
        var jsonData = File.ReadAllText(jsonPath);
        var jobject = JObject.Parse(jsonData);
        var invalidList = jobject["MandarotyRegistrations"].ToObject<List<RegistrationFormDTO>>();

        foreach (var data in invalidList)
        {
            yield return new TestCaseData(data).SetName($"InvalidRegistration_{data.FirstName}_{data.LastName}");
        }
    }
}