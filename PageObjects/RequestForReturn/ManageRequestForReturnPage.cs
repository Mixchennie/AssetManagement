using System.Globalization;
using System.Text.Json.Nodes;
using AssetManagement.Component;
using AssetManagementTest.Core.Browser;
using AssetManagementTest.Core.Element;
using AssetManagementTest.DataObject;
using AssetManagementTest.PageObjects;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AssetManagementTest.PageObjects.RequestForReturn;

class ManageRequestForReturnPage : BasePage
{
    private WebObject _menuManageRequestForReturn = new WebObject(By.XPath("//li[@class='sidebar-nav__item ' and normalize-space()='Request for Returning']"), "ManageRequestForReturnMenu");
    private WebObject _searchInput = new WebObject(
        By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]/preceding-sibling::input"), "UserSearchInput");
    private WebObject _searchButton = new WebObject(
        By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]"), "SearchButton");
    private WebObject _requestTableRows = new WebObject(By.XPath("//table//tbody/tr"), "RequestTableRows");
    private WebObject _sayYesRequestForReturnButton = new WebObject(
        By.XPath("//button[text()='Yes']"), "SayYesRequestForReturnButton");
    public void SearchAsset(string keyword)
    {
        _searchInput.SendKeysToElement(keyword);
        _searchButton.ClickOnElement();
    }
    public void GoToRequestForReturnManagement()
    {
        _menuManageRequestForReturn.ClickOnElement();
    }

    public string ConfirmRequestForReturn()
    {
        var driver = BrowserFactory.GetWebDriver();

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var assetcode = wait.Until(drv =>
        {
            var rows = _requestTableRows.GetElements();
            foreach (var row in rows)
            {
                var cols = row.FindElements(By.TagName("td"));
                if (cols.Count < 8) continue;
                bool match =
                    string.Equals(cols[7].Text.Trim(), "Waiting for returning".Trim(), StringComparison.OrdinalIgnoreCase);
                if (match)
                {
                    row.FindElement(By.XPath(".//i[@id='complete']")).Click();
                    _sayYesRequestForReturnButton.ClickOnElement();
                    return cols[1].Text.Trim();
                }
            }
            return null;
        });
        return assetcode;
    }

    public bool IsNoResultsFoundDisplayed()
    {
        var rows = _menuManageRequestForReturn.GetElements();
        if (!rows.Any()) return false;
        var firstRow = rows.First();
        var cells = firstRow.FindElements(By.TagName("td"));
        if (!cells.Any()) return false;
        var cellText = cells[0].Text.Trim();
        return cellText.Equals("No results found", StringComparison.OrdinalIgnoreCase)
            || cellText.Contains("no results", StringComparison.OrdinalIgnoreCase);
    }

public bool IsNoAssetFoundWithWaitForReturnDisplayed(string assetCode)
    {
        var driver = BrowserFactory.GetWebDriver();
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));

        var found = wait.Until(drv =>
        {
            var rows = _requestTableRows.GetElements();
            foreach (var row in rows)
            {
                var cols = row.FindElements(By.TagName("td"));
                if (cols.Count < 8) continue;
                bool match =
                    string.Equals(cols[1].Text.Trim(), assetCode.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[7].Text.Trim(), "Waiting for returning".Trim(), StringComparison.OrdinalIgnoreCase);
                if (match) return false; 
            }
            return true;
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
    
}