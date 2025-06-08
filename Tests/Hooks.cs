using SeleniumPractice.Utils;

namespace SeleniumPractice.Tests
{
    [SetUpFixture]
    public class Hooks
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            TestContext.Progress.WriteLine("OneTimeSetUp: Initializing WebDriver and navigating to URL.");
            ConfigurationUtils.ReadConfiguration("Configurations\\appsettings.json");
            
            ExtentReportHelper.InitializeReport(Directory.GetCurrentDirectory() + "\\Reports\\TestReport.html"
                , ConfigurationUtils.GetConfigurationByKey("HostName")
                , ConfigurationUtils.GetConfigurationByKey("Environment")
                , ConfigurationUtils.GetConfigurationByKey("Browser"));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestContext.Progress.WriteLine("OneTimeTearDown: Cleaning up WebDriver.");
            ExtentReportHelper.Flush();
        }
    }
}
