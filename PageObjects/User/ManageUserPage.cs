using System.Globalization;
using System.Text.Json.Nodes;
using AssetManagement.Component;
using AssetManagementTest.Core.Browser;
using AssetManagementTest.Core.Element;
using AssetManagementTest.DataObject;
using AssetManagementTest.PageObjects;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AssetManagementTest.PageObjects.User;

class ManageUserPage : BasePage
{
    private WebObject _menuManageUser = new WebObject(By.XPath("//li[@class='sidebar-nav__item ' and normalize-space()='Manage User']"), "ManageUserMenu");
    private WebObject _btnCreateNewUser = new WebObject(By.XPath("//button[normalize-space()='Create new user']"), "CreateNewUserButton");
    private WebObject _searchInput = new WebObject(
        By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]/preceding-sibling::input"), "UserSearchInput");
    private WebObject _searchButton = new WebObject(
        By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]"), "SearchButton");
    private WebObject _userTableRows = new WebObject(By.XPath("//table//tbody/tr"), "UserTableRows");

    private WebObject _disableUserButton = new WebObject(
        By.XPath("//button[text()='Disable']"), "DisableUserButton");
    public void SearchUser(string keyword)
    {
        _searchInput.SendKeysToElement(keyword);
        _searchButton.ClickOnElement();
    }

    public void GoToCreateUserForm()
    {
        _btnCreateNewUser.ClickOnElement();
    }

    public bool IsUserNotDisplayed(string fullName, string joinedDate, string type)
    {
        var driver = BrowserFactory.GetWebDriver();
        DateTime parsed;
        string joinedDateToCompare = joinedDate;
        if (DateTime.TryParseExact(joinedDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
        {
            joinedDateToCompare = parsed.ToString("yyyy-MM-dd");
        }

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        bool notFound = wait.Until(drv =>
        {
            var rows = _userTableRows.GetElements();
            foreach (var row in rows)
            {
                var cols = row.FindElements(By.TagName("td"));
                if (cols.Count < 5) continue;
                bool match =
                    string.Equals(cols[1].Text.Trim(), fullName.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[3].Text.Trim(), joinedDateToCompare.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[4].Text.Trim(), type.Trim(), StringComparison.OrdinalIgnoreCase);
                if (match) return false; 
            }
            return true; 
        });
        return notFound;
    }

    public void GoToUserManagement()
    {
        _menuManageUser.ClickOnElement();
    }

    public void DisableUser(string fullName, string joinedDate, string type)
    {
        var driver = BrowserFactory.GetWebDriver();
        DateTime parsed;
        string joinedDateToCompare = joinedDate;
        if (DateTime.TryParseExact(joinedDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
        {
            joinedDateToCompare = parsed.ToString("yyyy-MM-dd");
        }

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(drv =>
        {
            var rows = _userTableRows.GetElements();
            foreach (var row in rows)
            {
                var cols = row.FindElements(By.TagName("td"));
                if (cols.Count < 5) continue;
                bool match =
                    string.Equals(cols[1].Text.Trim(), fullName.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[3].Text.Trim(), joinedDateToCompare.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[4].Text.Trim(), type.Trim(), StringComparison.OrdinalIgnoreCase);
                if (match)
                {
                    row.FindElement(By.XPath(".//i[@id='disable']")).Click();
                    _disableUserButton.ClickOnElement();
                    return true;
                }
            }
            return false;
        });
    }
    public void SearchUserWithWait(string keyword)
    {
        WaitForTableToLoad();
        _searchInput.ClearElement();
        _searchInput.SendKeysToElement(keyword);
        _searchButton.ClickOnElement();
        WaitForTableToLoad();
    }

    public void WaitForTableToLoad()
    {
        var driver = BrowserFactory.GetWebDriver();
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(drv => _userTableRows.GetElements().Count > 0);
    }

    public bool IsUserDisplayed(string fullName, string joinedDate, string type)
    {
        var driver = BrowserFactory.GetWebDriver();
        DateTime parsed;
        string joinedDateToCompare = joinedDate;
        if (DateTime.TryParseExact(joinedDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
        {
            joinedDateToCompare = parsed.ToString("yyyy-MM-dd");
        }

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        bool found = wait.Until(drv =>
        {
            var rows = _userTableRows.GetElements();
            foreach (var row in rows)
            {
                var cols = row.FindElements(By.TagName("td"));
                if (cols.Count < 5) continue;
                Console.WriteLine($"[User Row] {string.Join(" | ", cols.Select(x => x.Text.Trim()))}");
                Console.WriteLine($"[Expected] Full Name: {fullName}, Joined Date: {joinedDateToCompare}, Type: {type}");
                bool match =
                    string.Equals(cols[1].Text.Trim(), fullName.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[3].Text.Trim(), joinedDateToCompare.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[4].Text.Trim(), type.Trim(), StringComparison.OrdinalIgnoreCase);
                if (match) return true;
            }
            return false;
        });
        return found;
    }

    public void WaitForLoadingSpinnerToDisappear()
    {
        var driver = BrowserFactory.GetWebDriver();
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(drv =>
        {
            var spinners = drv.FindElements(By.CssSelector(".spinner, .loading"));
            return spinners.All(s => !s.Displayed);
        });
    }


    public bool WaitForUserRemoved(string fullName, string joinedDate, string type, int timeoutSec = 8)
    {
        var driver = BrowserFactory.GetWebDriver();
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSec));

        var found = wait.Until(drv =>
        {
            var rows = _userTableRows.GetElements();
            foreach (var row in rows)
            {
                var cols = row.FindElements(By.TagName("td"));
                if (cols.Count < 5) continue;
                Console.WriteLine($"[User Row] {string.Join(" | ", cols.Select(x => x.Text.Trim()))}");
                Console.WriteLine($"[Expected] Full Name: {fullName}, Joined Date: {joinedDate}, Type: {type}");
                bool match =
                    string.Equals(cols[1].Text.Trim(), fullName.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[3].Text.Trim(), joinedDate, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[4].Text.Trim(), type, StringComparison.OrdinalIgnoreCase);
                if (match) return false;
            }
            return true;
        });

        return found;
    }

    public void ClickOnElement(WebObject webObject)
    {
        webObject.ClickOnElement();
    }

    public void SendKeysToElement(WebObject webObject, string text)
    {
        webObject.SendKeysToElement(text);
    }

    public void ClearElement(WebObject webObject)
    {
        webObject.ClearElement();
    }                                                   

    
    public bool IsNoResultsFoundDisplayed()
    {
        var rows = _userTableRows.GetElements();
        if (!rows.Any()) return false;
        var firstRow = rows.First();
        var cells = firstRow.FindElements(By.TagName("td"));
        if (!cells.Any()) return false;
        var cellText = cells[0].Text.Trim();
        return cellText.Equals("No results found", StringComparison.OrdinalIgnoreCase)
            || cellText.Contains("no results", StringComparison.OrdinalIgnoreCase);
    }
}