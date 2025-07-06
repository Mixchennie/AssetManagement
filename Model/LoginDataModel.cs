using System.Collections;
using AssetManagementTest.DataObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssetManagementTest.Model;
public static class LoginDataModel
{
    public static IEnumerable GetValidLoginTestData()
    {
        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "LoginData.json");
        var jsonData = File.ReadAllText(jsonPath);
        var jobject = JObject.Parse(jsonData);
        var validList = jobject["CredentialAccount"].ToObject<List<LoginDataDto>>();

        foreach (var data in validList)
        {
            yield return new TestCaseData(data).SetName($"ValidLogin_{data.Username}_{data.Password}");
        }
    }

    public static IEnumerable GetInvalidLoginUsernameTestData()
    {
        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "LoginData.json");
        var jsonData = File.ReadAllText(jsonPath);
        var jobject = JObject.Parse(jsonData);
        var invalidList = jobject["LoginWithInvalidUsername"].ToObject<List<LoginDataDto>>();

        foreach (var data in invalidList)
        {
            yield return new TestCaseData(data).SetName($"InvalidLogin_{data.Username}_{data.Password}");
        }
    }

    public static IEnumerable GetInvalidLoginPasswordTestData()
    {
        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Json", "LoginData.json");
        var jsonData = File.ReadAllText(jsonPath);
        var jobject = JObject.Parse(jsonData);
        var invalidList = jobject["LoginWithInvalidPassword"].ToObject<List<LoginDataDto>>();

        foreach (var data in invalidList)
        {
            yield return new TestCaseData(data).SetName($"InvalidLogin_{data.Username}_{data.Password}");
        }
    }
}