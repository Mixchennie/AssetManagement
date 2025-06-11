using AssetManagement.Component;
using AssetManagementTest.Core.Element;
using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Linq;

namespace AssetManagementTest.PageObjects.Assignment
{
    class CreateNewAssignmentPage : BasePage
    {
        private WebObject _menuAssignment = new WebObject(By.XPath("//li[@class='sidebar-nav__item ' and normalize-space()='Manage Assignment']"), "ManageAssignmentMenu");
        private WebObject _btnCreateNewAssignment = new WebObject(By.XPath("//button[normalize-space()='Create new assignment']"), "CreateNewAssignmentBtn");
        private WebObject _userSearchBtn = new WebObject(By.XPath("//label[contains(text(),'User')]/following-sibling::div[1]//i"), "UserSearchButton");
        private WebObject _assetSearchBtn = new WebObject(By.XPath("//label[contains(text(),'Asset')]/following-sibling::div[1]//i"), "AssetSearchButton");
        private WebObject _assignedDateInput = new WebObject(By.XPath("//label[contains(text(),'Assigned Date')]/following::button[2]"), "AssignedDateInput");
        private WebObject _noteInput = new WebObject(By.XPath("//label[contains(text(),'Note')]/following-sibling::div/textarea"), "NoteInput");
        private WebObject _saveButton = new WebObject(By.XPath("//button[normalize-space()='Save']"), "SaveButton");

        private WebObject _assignmentTableRows = new WebObject(By.XPath("//table//tbody/tr"), "AssignmentTableRows");

        private WebObject _searchInput = new WebObject(
    By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]/preceding-sibling::input"), "AssignmentSearchInput");

        private WebObject _searchButton = new WebObject(
            By.XPath("//button[.//i[contains(@class, 'fa-magnifying-glass')]]"), "SearchButton");

        public void GoToCreateAssignmentForm()
        {
            _menuAssignment.ClickOnElement();
            _btnCreateNewAssignment.ClickOnElement();
        }

        public void SelectUser(string fullName)
        {
            _userSearchBtn.ClickOnElement();
            SelectFromModal(fullName, "Select User");
        }

        public void SelectAsset(string assetName)
        {
            _assetSearchBtn.ClickOnElement();
            SelectFromModal(assetName, "Select Asset");
        }

        private void SelectFromModal(string keyword, string modalTitle)
        {
            var searchInput = new WebObject(By.XPath("//div[contains(@class, 'modal')]//input[@type='text']"), $"{modalTitle}SearchInput");
            var searchBtn = new WebObject(By.XPath("//div[contains(@class, 'modal')]//button[.//i[contains(@class, 'fa-magnifying-glass')]]"), $"{modalTitle}SearchBtn");

            searchInput.SendKeysToElement(keyword);
            searchBtn.ClickOnElement();

            var userRowRadio = new WebObject(By.XPath($"//div[contains(@class, 'modal')]//td[contains(text(), '{keyword}')]/preceding-sibling::td//div[contains(@class, 'rounded-full')]"), "RowRadioBtn");
            userRowRadio.ClickOnElement();

            var saveModalBtn = new WebObject(By.XPath("//div[contains(@class,'modal')]//button[normalize-space()='Save']"), "ModalSaveBtn");
            saveModalBtn.ClickOnElement();
        }

        public void SetAssignedDate(string date)
        {
            _assignedDateInput.ClickOnElement();
            DatePickerComponent.SetReactDatePicker(date);
        }

        public void EnterNote(string note)
        {
            _noteInput.SendKeysToElement(note);
        }

        public void ClickSaveButton()
        {
            _saveButton.ClickOnElement();
        }

        public void SearchAssignment(string keyword)
        {
            _searchInput.SendKeysToElement(keyword);
            _searchButton.ClickOnElement();
        }

        public bool IsAssignmentDisplayed(string assetName, string assignedDate)
        {
            var rows = _assignmentTableRows.GetElements();
            DateTime parsed;
            string assignedDateToCompare = assignedDate;
            if (DateTime.TryParseExact(assignedDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            {
                assignedDateToCompare = parsed.ToString("yyyy-MM-dd");
            }

            foreach (var row in rows)
            {
                var cols = row.FindElements(By.TagName("td"));
                if (cols.Count < 5) continue;
                Console.WriteLine($"[User Row] {string.Join(" | ", cols.Select(x => x.Text.Trim()))}");

                bool match =
                    string.Equals(cols[5].Text.Trim(), assignedDateToCompare.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cols[2].Text.Trim(), assetName.Trim(), StringComparison.OrdinalIgnoreCase);

                if (match) return true;
            }
            return false;
        }
    }
}
