using OpenQA.Selenium;
using PracticeSelenium.Pages.WebPages;

class BookStorePage : BasePage
{
    private WebObject _searchBox = new WebObject(By.Id("searchBox"), "SearchBox");
    private WebObject _resultRows = new WebObject(By.XPath("//div[@class='rt-tbody']//div[@class='rt-tr-group']"), "ResultRows");
    private WebObject _nextPageBtn = new WebObject(By.XPath("//button[text()='Next']"), "NextPageBtn");
    
    public BookStorePage() : base()
    {
    }

    public void EnterToSearchBox(string keyword)
    {
        _searchBox.SendKeysToElement(keyword);
    }

    public void VerifyAllRowsContainKeyword(string keyword)
    {
        _resultRows.ScrollToElement();
        while (true)
        {
            AssertRowContainsKeyword(_resultRows, keyword);
            
            if (_nextPageBtn.IsDisabled())
                break;
            else
                _nextPageBtn.ClickOnElement();
        }
    }
}