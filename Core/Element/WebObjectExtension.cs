using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPractice.Utils;

namespace AssetManagementTest.Core.Element;
public static class WebObjectExtensions
{
    private static WebDriverWait Wait => new WebDriverWait(BrowserFactory.GetWebDriver(),
    TimeSpan.FromSeconds(long.Parse(ConfigurationUtils.GetConfigurationByKey("WebDriver.Wait.Time"))));

    public static IWebElement WaitForElementToBeVisible(this WebObject webObject)
    {
        Wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        return Wait.Until(driver =>
        {
            var element = driver.FindElement(webObject.By);
            return (element.Displayed) ? element : null;
        });
    }

    public static IWebElement WaitForElementToBeInteractable(this WebObject webObject, int timeoutSeconds = 10)
    {
        var driver = BrowserFactory.GetWebDriver();
        Wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        return Wait.Until(d =>
        {
            var element = d.FindElement(webObject.By);
            return (element.Displayed && element.Enabled) ? element : null;
        });
    }

    public static IWebElement WaitForElementToBeClickable(this WebObject webObject)
    {
        Wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        return Wait.Until(driver =>
        {
            var element = driver.FindElement(webObject.By);
            return (element != null && element.Enabled && element.Displayed) ? element : null;
        });
    }

    public static void ClickOnElement(this WebObject webObject)
    {
        var element = webObject.WaitForElementToBeClickable();
        element.Click();
    }

    public static void SendKeysToElement(this WebObject webObject, string text)
    {
        var element = webObject.WaitForElementToBeVisible();
        element.Clear();
        element.SendKeys(text);
    }

    public static void ClearElement(this WebObject webObject)
    {
        var element = webObject.WaitForElementToBeVisible();
        element.Clear();
    }

    public static string GetTextFromElement(this WebObject webObject)
    {
        return webObject.WaitForElementToBeVisible().Text;
    }

    public static void SelectDropdownByIndex(this WebObject webObject, int index)
    {
        IWebElement element = webObject.WaitForElementToBeClickable();
        var select = new SelectElement(element);
        select.SelectByIndex(index);
    }

    public static void SelectDropdownByText(this WebObject webObject, string value)
    {
        var element = webObject.WaitForElementToBeClickable();
        var selectElement = new SelectElement(element);
        selectElement.SelectByText(value);
    }

    public static void ScrollToElement(this WebObject webObject)
    {
        var driver = BrowserFactory.GetWebDriver();

        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
        var element = webObject.WaitForElementToBeVisible();
        jsExecutor.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'auto', block: 'center' });", element);
    }

    public static int GetElementCount(this WebObject webObject)
    {
        var elements = BrowserFactory.GetWebDriver().FindElements(webObject.By);
        return elements.Count;
    }

    public static IReadOnlyCollection<IWebElement> GetElements(this WebObject webObject)
    {
        var elements = BrowserFactory.GetWebDriver().FindElements(webObject.By);
        return elements;
    }

    public static List<WebObject> GetWebObjects(this WebObject webObject)
    {
        var elements = BrowserFactory.GetWebDriver().FindElements(webObject.By);
        var webObjectsList = elements.Select(element => new WebObject(webObject.By, webObject.Name)).ToList();
        return webObjectsList;
    }

    public static bool VerifyColumnContainsKeyword(this WebObject columnCells, string keyword)
    {
        var elements = columnCells.GetElements();
        foreach (var element in elements)
        {
            var text = element.Text.Trim().ToLower();
            if (!text.Contains(keyword.ToLower()))
            {
                return false;
            }
        }
        return true;
    }

    public static bool VerifyRowsContainKeyword(this WebObject webObjectRows, string keyword)
    {
        webObjectRows.ScrollToElement();
        var elements = webObjectRows.GetElements();
        var loweredKeyword = keyword.ToLower();

        foreach (var element in elements)
        {
            var actualText = element.Text?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(actualText) && !actualText.Contains(loweredKeyword))
                return false;
        }
        return true;
    }

    public static bool IsDisabled(this WebObject webObject)
    {
        var element = webObject.WaitForElementToBeVisible();
        try
        {
            return element.GetAttribute("disabled") != null;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public static void PressEnter(this WebObject webObject)
    {
        webObject.WaitForElementToBeVisible().SendKeys(Keys.Enter);
    }
}