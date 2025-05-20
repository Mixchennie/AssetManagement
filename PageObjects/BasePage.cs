using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PracticeSelenium.Pages.WebPages;
class BasePage
{
    public BasePage()
    {
    }

    public void SelectDateWithDatePicker(WebObject webObject, string date)
    {
        string[] dateArray = date.Split(' ');
        DateTime targetDate = DateTime.ParseExact(date, "d MMMM yyyy", CultureInfo.InvariantCulture);
        string expectedLabel = "Choose "
            + targetDate.ToString("dddd, MMMM d", CultureInfo.InvariantCulture)
            + GetDaySuffix(targetDate.Day)
            + ", " + targetDate.Year;

        WebObject datepicker = new WebObject(By.XPath("//div[@id='dateOfBirth']/div"), "DatePicker");
        datepicker.ClickOnElement();

        WebObject monthDropdown = new WebObject(By.XPath("//div[@id='dateOfBirth']//select[contains(@class, 'month-select')]"), "MonthDropdown");
        WebObject yearDropdown = new WebObject(By.XPath("//div[@id='dateOfBirth']//select[contains(@class, 'year-select')]"), "YearDropdown");
        WebObject dayPicker = new WebObject(By.XPath($"//div[@aria-label='{expectedLabel}']"), "DayPicker");

        monthDropdown.SelectDropdownByText(dateArray[1]);
        yearDropdown.SelectDropdownByText(dateArray[2]);
        dayPicker.ClickOnElement();
    }

    private static string GetDaySuffix(int day)
    {
        if (day >= 11 && day <= 13) return "th";
        return (day % 10) switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
    }

    public static void AssertTitleContainKeyword(WebObject webObject, string keyword)
    {
        List<string> allTitles = webObject.GetElements().Select(e => e.Text.Trim()).ToList();

        for (int i = 0; i < allTitles.Count; i++)
        {
            string title = allTitles[i];
            Assert.That(title,
                Does.Contain(keyword).IgnoreCase,
                $"❌ Row {i + 1}: Expected to contain \"{keyword}\" but was \"{title}\"");
        }
    }

    public static void AssertRowContainsKeyword(WebObject webObject, string keyword, int rowIndex = -1)
    {
        var cells = webObject.GetElements().ToList();
        bool containsKeyword = cells.Any(cell =>
            cell.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        if (cells.All(cell => string.IsNullOrWhiteSpace(cell.Text)))
        {
            return;
        }

        if (!containsKeyword)
        {
            string allCellsText = string.Join("\n", cells.Select((c, i) =>
                $"- [Row {i + 1}]: \"{c.Text.Trim()}\""));

            string rowInfo = rowIndex >= 0 ? $"❌ Row {rowIndex + 1}" : "❌ Row";
            string message =
                $"{rowInfo} does not contain keyword: \"{keyword}\"\n" +
                $"📄 Row content:\n{allCellsText}";

            Assert.Fail(message);
        }
    }

    public static void VerifyCellsDoNotContainKeyword(WebObject webObject, string keyword, int rowIndex = -1)
    {
        var cells = webObject.GetElements().ToList();

        if (cells.All(cell => string.IsNullOrWhiteSpace(cell.Text)))
        {
            return;
        }

        var cellsContainingKeyword = cells
        .Select((cell, index) => new { cell, index })
        .Where(x => x.cell.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        .ToList();

        if (cellsContainingKeyword.Any())
        {
            string allCellsText = string.Join("\n", cells.Select((c, i) =>
                $"- [Row {i + 1}]: \"{c.Text.Trim()}\""));

            string violatingCellsText = string.Join("\n", cellsContainingKeyword.Select(x =>
                $"[Row {x.index + 1}] contains keyword: \"{keyword}\": \"{x.cell.Text.Trim()}\""));

            string rowInfo = rowIndex >= 0 ? $"Row {rowIndex + 1}" : "Row";
            string message =
                $"{rowInfo} contains keyword: \"{keyword}\"\n" +
                $"Details of violating cells:\n{violatingCellsText}\n\n" +
                $"Content of Cells:\n{allCellsText}";

            Assert.Fail(message);
        }
    }
    public static void SelectDropdownOptionByText(string optionText)
    {
        var driver = BrowserFactory.GetWebDriver();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        var dropdownOptions = wait.Until(drv => drv.FindElements(By.CssSelector("div[class*='menu'] div[class*='option']")));

        foreach (var option in dropdownOptions)
        {
            if (option.Text.Trim().Equals(optionText, StringComparison.OrdinalIgnoreCase))
            {
                option.Click();
                return;
            }
        }
        throw new Exception($"Can not find option with text: {optionText}");
    }

    public static void DeleteBookByTitle(string bookTitle)
    {
        try
        {
            string xpath = $"//div[.//a[text()='{bookTitle}']]//span[@id='delete-record-undefined']";
            WebObject deleteIcon = new WebObject(By.XPath(xpath), "DeleteIcon");

            deleteIcon.WaitForElementToBeVisible();
            deleteIcon.ScrollToElement();
            deleteIcon.ClickOnElement();

        }
        catch (NoSuchElementException)
        {
            Console.WriteLine($"Can not find book with text: {bookTitle}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when deleting: {ex.Message}");
        }
    }

    public static void HandleAlert(string expectedText = null, bool accept = true, int timeoutInSeconds = 10)
    {
        var driver = BrowserFactory.GetWebDriver();
        try
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IAlert alert = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());

            if (!string.IsNullOrEmpty(expectedText))
            {
                if (!alert.Text.Contains(expectedText))
                    throw new Exception($"Alert text does not match expected. Expected: {expectedText}, Actual: {alert.Text}");
            }

            if (accept)
                alert.Accept();
            else
                alert.Dismiss();
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Alert was not present within the timeout period.");
        }
    }

    public static WebObject GetColumnCellsByHeaderText(WebObject tableObject, string headerText)
    {
        var tableElement = BrowserFactory.GetWebDriver().FindElement(tableObject.By);

        var headerElements = tableElement.FindElements(By.XPath("//div[@role='columnheader']"));

        int columnIndex = headerElements
            .Select((header, index) => new { header, index })
            .FirstOrDefault(h => h.header.Text.Trim().Equals(headerText, StringComparison.OrdinalIgnoreCase))
            ?.index ?? -1;

        Console.WriteLine(columnIndex);

        if (columnIndex == -1)
            throw new Exception($"Header with text '{headerText}' not found.");

        var columnCells = new WebObject(By.XPath($"//div[@class='rt-tbody']//div[@class='rt-td'][{columnIndex + 1}]"), "TitleColumn");

        return columnCells;
    }
}