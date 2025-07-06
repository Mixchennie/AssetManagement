using AssetManagementTest.Core.Element;
using AssetManagementTest.PageObjects;
using OpenQA.Selenium;

namespace AssetManagementTest.PageObjects;

class LoginPage : BasePage
{
    private WebObject _userNameInput = new WebObject(By.Id("Username"), "UserNameInput");
    private WebObject _passwordInput = new WebObject(By.Id("Password"), "PasswordInput");
    private WebObject _loginBtn = new WebObject(By.XPath("//button[@type='submit']"), "LoginBtn");
    private WebObject _loginErrorMessage = new WebObject(By.CssSelector("div[ng-show = 'isError']"), "LoginErrorMessage");
    private WebObject _navigateBarTitle = new WebObject(By.XPath("//header[@class='header']//h1"), "NavigateBarTitle");
    public LoginPage() : base()
    {
    }

    public void EnterUserName(string userName)
    {
        _userNameInput.SendKeysToElement(userName);
    }
    public void EnterPassword(string password)
    {
        _passwordInput.SendKeysToElement(password);
    }

    public void DoLogin(string userName, string password)
    {
        EnterUserName(userName);
        EnterPassword(password);
        ClickLoginButton();
    }

    public void ClickLoginButton()
    {
        _loginBtn.ClickOnElement();
    }

    public string GetLoginErrorMessage()
    {
        return _loginErrorMessage.GetTextFromElement();
    }

    public void VerifyLoginSuccessMessage(string expectedMessage)
    {
        HandleToastMessage(expectedMessage);
    }

    public string GetNavigateBarTitleText()
    {
        _navigateBarTitle.WaitForElementToBeVisible();
        return _navigateBarTitle.GetTextFromElement().Trim();
    }
}