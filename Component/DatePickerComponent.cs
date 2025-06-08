using System.Globalization;
using AssetManagementTest.Core.Element;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AssetManagement.Component
{
    public class DatePickerComponent
    {
        private readonly WebObject _rootDatePicker;
        private readonly WebObject _monthDropdown;
        private readonly WebObject _yearDropdown;

        public DatePickerComponent(By rootLocator)
        {
            _rootDatePicker = new WebObject(rootLocator, "RootDatePicker");

            _monthDropdown = new WebObject(
                By.XPath($"{rootLocator}//select[contains(@class, 'month-select')]"),
                "MonthDropdown");

            _yearDropdown = new WebObject(
                By.XPath($"{rootLocator}//select[contains(@class, 'year-select')]"),
                "YearDropdown");
        }

        public void Open()
        {
            _rootDatePicker.ClickOnElement();
        }

        public void SelectMonth(string monthName)
        {
            _monthDropdown.SelectDropdownByText(monthName);
        }

        public void SelectYear(string yearValue)
        {
            IReadOnlyCollection<WebObject> optionWebObjects = _yearDropdown.GetWebObjects();

            var yearOptions = new List<int>();
            foreach (var option in optionWebObjects)
            {
                string text = option.GetTextFromElement().Trim();
                if (int.TryParse(text, out int y))
                    yearOptions.Add(y);
            }

            if (!yearOptions.Any())
                throw new InvalidOperationException("Year Dropdown does not contain any valid year options.");

            int minYear = yearOptions.Min();
            int maxYear = yearOptions.Max();

            if (!int.TryParse(yearValue, out int targetYear))
                throw new ArgumentException($"Year value \"{yearValue}\" is not valid. It must be an integer.");

            int yearToSelect = targetYear;
            if (targetYear < minYear) yearToSelect = minYear;
            else if (targetYear > maxYear) yearToSelect = maxYear;

            _yearDropdown.SelectDropdownByText(yearToSelect.ToString());

            if (yearToSelect != targetYear)
            {
                TestContext.Progress.WriteLine(
                    $"[DatePicker] Year \"{targetYear}\" is out of range [{minYear}, {maxYear}]. " +
                    $"Automatically selected \"{yearToSelect}\".");
            }
        }

        public void SelectDay(DateTime date)
        {
            string expectedLabel = "Choose "
                + date.ToString("dddd, MMMM d", CultureInfo.InvariantCulture)
                + GetDaySuffix(date.Day)
                + ", " + date.Year;

            By dayLocator = By.XPath($"//div[@aria-label=\"{expectedLabel}\"]");

            var dayWebObject = new WebObject(dayLocator, "DayPicker");
            dayWebObject.ClickOnElement();
        }

        private static string GetDaySuffix(int day)
        {
            if (day >= 11 && day <= 13)
                return "th";

            return (day % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        }

        public void SetDate(string dateString)
        {
            string[] parts = dateString.Split(' ');
            if (parts.Length != 3)
                throw new ArgumentException($"Invalid date format. Expected: \"d MMMM yyyy\", but got: \"{dateString}\"");

            DateTime targetDate = DateTime.ParseExact(
                dateString,
                "d MMMM yyyy",
                CultureInfo.InvariantCulture);

            Open();

            SelectYear(parts[2]);
            SelectMonth(parts[1]);
            SelectDay(targetDate);
        }

        public string GetSelectedDateValue()
        {
            return _rootDatePicker.GetTextFromElement().Trim();
        }

        public bool VerifySelectedDate(string expectedDate)
        {
            string actualValue = GetSelectedDateValue();
            return actualValue.Equals(expectedDate, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}