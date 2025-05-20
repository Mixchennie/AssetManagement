
using SeleniumPractice.PageObjects;
using SeleniumPractice.TestData.DTO;
using SeleniumPractice.Tests;
using SeleniumPractice.Utils;

namespace SeleniumPractice.Tests
{
    class BookStorePageTest : BaseTest
    {
        [TestCase("Design")]
        [TestCase("De")]
        [Category("SearchTest")]
        [Description("Verify searching books returns rows matching the keyword")]
        public void SearchBookSuccessfully(string keyword)
        {
            ExtentReportHelper.LogTestStep("Go to book store page");
            DriverUtils.GoToUrl(ConfigurationUtils.GetConfigurationByKey("SearchBookUrl"));
            var bookStorePage = new BookStorePage();

            ExtentReportHelper.LogTestStep("Enter keyword to search box");
            bookStorePage.EnterToSearchBox(keyword);

            ExtentReportHelper.LogTestStep("Verify all rows contain keyword");
            Assert.Multiple(() =>
            {
                bookStorePage.VerifyAllRowsContainKeyword(keyword);
            });
        }
    }
}