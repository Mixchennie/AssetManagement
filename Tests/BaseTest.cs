using AssetManagementTest.Core.Browser;
using AssetManagementTest.Report;
using AssetManagementTest.Utils;

namespace AssetManagementTest.Tests
{
    public class BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            TestContext.Progress.WriteLine("Setup: Initializing WebDriver, Maximize Window, and Extent Report.");
 
            BrowserFactory.InitializeDriver(ConfigurationUtils.GetConfigurationByKey("Browser"));
            DriverUtils.MaximizeWindow();
            DriverUtils.GoToUrl(ConfigurationUtils.GetConfigurationByKey("SearchBookUrl"));

            ExtentReportHelper.CreateTest(TestContext.CurrentContext.Test.Name);
            ExtentReportHelper.CreateNode(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace) ? "" : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            
            ExtentReportHelper.CreateTestResult(status, stacktrace, TestContext.CurrentContext.Test.ClassName, stacktrace);
            
            TestContext.Progress.WriteLine("TearDown: Cleaning up WebDriver.");
            
            BrowserFactory.CleanUpWebDriver();
        }
    }
}