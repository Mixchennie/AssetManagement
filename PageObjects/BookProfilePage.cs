using OpenQA.Selenium;
using PracticeSelenium.Pages.WebPages;

class BookProfilePage : BasePage
{
    private WebObject _loginLink = new WebObject(By.XPath("//label[@id='notLoggin-label']/a[text()='login']"), "LoginLink");
    private WebObject _usernameInput = new WebObject(By.Id("userName"), "UsernameInput");
    private WebObject _passwordInput = new WebObject(By.Id("password"), "PasswordInput");
    private WebObject _loginButton = new WebObject(By.Id("login"), "LoginButton");
    private WebObject _deleteComfirmBtn = new WebObject(By.Id("closeSmallModal-ok"), "DeleteComfirmBtn");
    private WebObject _bookList = new WebObject(By.XPath("//div[@class='rt-table']"), "BookList");
    private WebObject _titleColumnCells;
    private WebObject _nextPageBtn = new WebObject(By.XPath("//button[text()='Next']"), "NextPageBtn");
    
    public BookProfilePage() : base()
    {

    }

    public void EnterUsername(string username)
    {
        _usernameInput.ScrollToElement();
        _usernameInput.SendKeysToElement(username);
    }

    public void EnterPassword(string password)
    {
        _passwordInput.ScrollToElement();
        _passwordInput.SendKeysToElement(password);
    }

    public void ClickLoginButton()
    {
        _loginButton.ScrollToElement();
        _loginButton.ClickOnElement();
    }

    public void LoginAccount(string username, string password)
    {
        _loginLink.ScrollToElement();
        _loginLink.ClickOnElement();
        EnterUsername(username);
        EnterPassword(password);
        ClickLoginButton();
    }
    public void DeleteBook(string bookTitle)
    {
        DeleteBookByTitle(bookTitle);
        _deleteComfirmBtn.ClickOnElement();
        HandleAlert("Book deleted.");
    }

    public void VerifyTableDoesNotContainsBook(string bookTitle)
    {
        _titleColumnCells = GetColumnCellsByHeaderText(_bookList, "Title");
        while (true)
        {
            VerifyCellsDoNotContainKeyword(_titleColumnCells, bookTitle);

            if (_nextPageBtn.IsDisabled())
                break;
            else
                _nextPageBtn.ClickOnElement();
        }
    }
}