using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

public static class BrowserFactory
{
    public static ThreadLocal<IWebDriver> ThreadLocalWebDriver = new ThreadLocal<IWebDriver>();

    public static void InitializeDriver(string browserName, bool isHeadless = false)
    {
        browserName = browserName.ToLower();

        switch (browserName)
        {
            case "chrome":
                new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--start-maximized");
                chromeOptions.AddArgument("--disable-notifications");
                if (isHeadless)
                {
                    chromeOptions.AddArgument("--headless");
                }
                chromeOptions.AddArgument("--no-sandbox");
                chromeOptions.AddArgument("--disable-dev-shm-usage");

                ThreadLocalWebDriver.Value = new ChromeDriver(chromeOptions);
                break;

            case "firefox":
                new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
                var firefoxOptions = new FirefoxOptions();
                if (isHeadless)
                {
                    firefoxOptions.AddArgument("--headless");
                }
                firefoxOptions.AddArgument("--no-sandbox");

                ThreadLocalWebDriver.Value = new FirefoxDriver(firefoxOptions);
                break;

            case "edge":
                new DriverManager().SetUpDriver(new EdgeConfig(), VersionResolveStrategy.MatchingBrowser);
                var edgeOptions = new EdgeOptions();
                if (isHeadless)
                {
                    edgeOptions.AddArgument("headless");
                }

                ThreadLocalWebDriver.Value = new EdgeDriver(edgeOptions);
                break;

            default:
                throw new ArgumentException($"Browser '{browserName}' is not supported.");
        }

        ThreadLocalWebDriver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    public static IWebDriver GetWebDriver()
    {
        return ThreadLocalWebDriver.Value ?? throw new NullReferenceException("WebDriver is not initialized. Call InitializeDriver first.");
    }

    public static void CleanUpWebDriver()
    {
        if (ThreadLocalWebDriver.Value != null)
        {
            ThreadLocalWebDriver.Value.Quit();
            ThreadLocalWebDriver.Value.Dispose();
        }
    }
}
