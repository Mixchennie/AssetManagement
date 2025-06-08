using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using SeleniumPractice.Utils;
using AventStack.ExtentReports.Model;

public class ExtentReportHelper
{
    static AventStack.ExtentReports.ExtentReports _extentManager;

    [ThreadStatic]
    public static ExtentTest _extentTest;

    [ThreadStatic]
    public static ExtentTest _node;

    public static void InitializeReport(string reportPath, string hostName, string environment, string browser)
    {
        var htmlReporter = new ExtentSparkReporter(reportPath);
        _extentManager = new AventStack.ExtentReports.ExtentReports();
        _extentManager.AttachReporter(htmlReporter);
        _extentManager.AddSystemInfo("Host Name", hostName);
        _extentManager.AddSystemInfo("Environment", environment);
        _extentManager.AddSystemInfo("Browser", browser);
    }

    public static void Flush()
    {
        Console.WriteLine("before flush");
        _extentManager.Flush();
        Console.WriteLine("after flush");
    }

    public static void CreateTest(string name)
    {
        _extentTest = _extentManager.CreateTest(name);
        Console.WriteLine("create test");
    }

    public static void CreateNode(string name)
    {
        _node = _extentTest.CreateNode(name);
        Console.WriteLine("create node");
    }

    public static void LogTestStep(string stepName)
    {
        _node.Info(stepName);
    }

    public static void CreateTestResult(TestStatus status, string stacktrace, string className, string testName)
    {
        Status logstatus;
        ExtentTest target = _node ?? _extentTest;
        switch (status)
        {
            case TestStatus.Failed:
                logstatus = Status.Fail;
                if (BrowserFactory.GetWebDriver() != null)
                {
                    try
                    {
                        var mediaEntity = CaptureScreenShotAndAttachToExtendReport(BrowserFactory.GetWebDriver(), testName);
                        target.Fail($"#Test Name: {testName} #Status: {logstatus}\n{stacktrace}", mediaEntity);
                    }
                    catch (Exception ex)
                    {
                        target.Fail($"#Test Name: {testName} #Status: {logstatus}\n{stacktrace}\nScreenshot Failed: {ex.Message}");
                    }
                }
                else
                {
                    target.Fail("#Test Name: " + testName + " #Status: " + logstatus + stacktrace);
                }
                break;
            case TestStatus.Inconclusive:
                logstatus = Status.Warning;
                target.Log(logstatus, "#Test Name: " + testName + " #Status: " + logstatus);
                break;
            case TestStatus.Skipped:
                logstatus = Status.Skip;
                target.Skip("#Test Name: " + testName + " #Status: " + logstatus);
                break;
            default:
                logstatus = Status.Pass;
                if (BrowserFactory.GetWebDriver() != null)
                {
                    try
                    {
                        var mediaEntity = CaptureScreenShotAndAttachToExtendReport(BrowserFactory.GetWebDriver(), testName);
                        target.Log(logstatus, $"#Test Name: {testName} #Status: {logstatus}", mediaEntity);
                    }
                    catch (Exception ex)
                    {
                        string log = $"#Test Name: {testName ?? "Unknown"} #Status: {logstatus}\n" +
                        $"{stacktrace ?? ex.StackTrace ?? "No stacktrace"}\n" +
                        $"Screenshot Failed: {ex.Message}";
                        target.Log(logstatus, log);
                    }
                }
                else
                {
                    target.Log(logstatus, $"#Test Name: {testName} #Status: {logstatus}");
                }
                break;
        }
    }

    public static string CaptureScreenshot(IWebDriver driver, string className, string testName)
    {
        ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
        Screenshot screenshot = screenshotDriver.GetScreenshot();
        string directory = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationUtils.GetConfigurationByKey("ScreenshotDirectory.folder"));

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"Screenshot_{className}_{testName}_{timestamp}.png";
        Directory.CreateDirectory(directory);
        string filePath = string.Format("{0}/{1}", directory, fileName);
        screenshot.SaveAsFile(filePath);
        Console.WriteLine($"Screenshot saved at: {filePath}");
        return filePath;
    }

    public static Media CaptureScreenShotAndAttachToExtendReport(IWebDriver driver, string screenShotName)
    {
        ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
        var screenshot = screenshotDriver.GetScreenshot().AsBase64EncodedString;
        return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();
    }
}
