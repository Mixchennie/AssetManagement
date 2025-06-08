using AssetManagementTest.Core.Element;
using OpenQA.Selenium;

namespace PracticeSelenium.Component;
public class NavigationComponent
{
    private WebObject _navigationBar = new WebObject(By.XPath("//nav[@class='navigation']"), "NavigationBar");
    private WebObject _navigateBarTitle = new WebObject(By.XPath("//header[@class='header']//h1"), "NavigateBarTitle");

    public void NavigateTo(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Navigation path cannot be null or empty.", nameof(path));

        var pathArray = path.Split(" > ").Select(s => s.Trim()).ToArray();

        foreach (var segment in pathArray)
        {
            NavigateToSegment(segment);
        }
    }

    private void NavigateToSegment(string segment)
    {
        try
        {
            WebObject linkElement = FindLinkElementByText(segment);
            linkElement.WaitForElementToBeVisible();
            linkElement?.ClickOnElement();
        }
        catch (NoSuchElementException ex)
        {
            throw new Exception($"Error navigating to '{segment}': {ex.Message}");
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception($"Timeout while waiting for '{segment}' link to be clickable.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while navigating to '{segment}': {ex.Message}");
        }
    }

    private WebObject FindLinkElementByText(string linkText)
    {
        var linkElements = BrowserFactory.GetWebDriver().FindElements(By.LinkText(linkText))
                           .Select(element => new WebObject(By.LinkText(element.Text), element.Text))
                           .ToList();

        return linkElements.FirstOrDefault();
    }

    public string GetNavigateBarTitleText()
    {
        return _navigateBarTitle.GetTextFromElement().Trim();
    }

}
