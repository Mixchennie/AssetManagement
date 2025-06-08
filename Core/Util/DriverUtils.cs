
using AventStack.ExtentReports;
using OpenQA.Selenium;
using SeleniumPractice.Utils;

public static class DriverUtils
{
    public static void MaximizeWindow()
    {
        BrowserFactory.GetWebDriver().Manage().Window.Maximize();
    }

    public static void GoToUrl(string url)
    {
        BrowserFactory.GetWebDriver().Navigate().GoToUrl(url);
    }
    
    public static string CaptureScreenshot(IWebDriver driver, string className, string testName )
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
}