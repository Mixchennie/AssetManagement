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

class EditUserPage : BasePage
{
    private WebObject _menuManageUser = new WebObject(By.XPath("//li[@class='sidebar-nav__item ' and normalize-space()='Manage User']"), "ManageUserMenu");
    private WebObject _btnCreateNewUser = new WebObject(By.XPath("//button[normalize-space()='Create new user']"), "CreateNewUserButton");
    private WebObject _firstNameInput = new WebObject(By.Id("firstName"), "FirstNameInput");
    private WebObject _lastNameInput = new WebObject(By.Id("lastName"), "LastNameInput");
    private WebObject _emailInput = new WebObject(By.Id("email"), "EmailInput");
    private WebObject _dobInput = new WebObject(By.Id("dob"), "DOBInput");
    private WebObject _joinedDateInput = new WebObject(By.Id("joinedDate"), "JoinedDateInput");
    private WebObject _searchInput = new WebObject(
        By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]/preceding-sibling::input"), "UserSearchInput");

    private WebObject _searchButton = new WebObject(
        By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]"), "SearchButton");

    private WebObject _userTableRows = new WebObject(By.XPath("//table//tbody/tr"), "UserTableRows");

    private WebObject _maleRadio = new WebObject(By.XPath("//input[@type='radio' and @value='MALE']"), "MaleRadio");
    private WebObject _femaleRadio = new WebObject(By.XPath("//input[@type='radio' and @value='FEMALE']"), "FemaleRadio");

    private WebObject _typeDropdown = new WebObject(By.XPath("//label[contains(text(),'Type')]/following-sibling::div/select"), "TypeDropdown");
    private WebObject _locationDropdown = new WebObject(By.XPath("//label[contains(text(),'Location')]/following-sibling::div/select"), "LocationDropdown");

    private WebObject _saveButton = new WebObject(By.XPath("//button[normalize-space()='Save']"), "SaveButton");

    public void SearchUser(string keyword)
    {
        _searchInput.SendKeysToElement(keyword);
        _searchButton.ClickOnElement();
    }

    public bool IsUserDisplayed(string fullName, string joinedDate, string type)
    {
        var rows = _userTableRows.GetElements();
        DateTime parsed;
        string joinedDateToCompare = joinedDate;
        if (DateTime.TryParseExact(joinedDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
        {
            joinedDateToCompare = parsed.ToString("yyyy-MM-dd");
        }

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
    }

    public void GoToUserManager()
    {
        _menuManageUser.ClickOnElement();
    }
    public void SelectGender(string gender)
    {
        if (gender.ToUpper() == "MALE")
            _maleRadio.ClickOnElement();
        else if (gender.ToUpper() == "FEMALE")
            _femaleRadio.ClickOnElement();
    }

    public void SelectDateOfBirth(string dob)
    {
        _dobInput.ClickOnElement();
        DatePickerComponent.SetReactDatePicker(dob);
    }

    public void SelectJoinedDate(string joinedDate)
    {
        _joinedDateInput.ClickOnElement();
        DatePickerComponent.SetReactDatePicker(joinedDate);
    }

    public void SelectType(string type) => _typeDropdown.SelectDropdownByText(type);
    public void SelectLocation(string location) => _locationDropdown.SelectDropdownByText(location);
    public void ClickSaveButton() => _saveButton.ClickOnElement();

    public void FillUserForm(EditUserDataDto user)
    {
        SelectGender(user.Gender);
        SelectDateOfBirth(user.DateOfBirth);
        SelectJoinedDate(user.JoinedDate);
        SelectType(user.Type);
    }

    public void ClickEditUserByInfo(string fullName, string joinedDate, string type)
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
                    row.FindElement(By.XPath(".//i[@id='edit' or contains(@class,'fa-pen')]")).Click();
                    return true;
                }
            }
            return false;
        });
    }
}