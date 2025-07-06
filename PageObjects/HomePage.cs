using AssetManagementTest.Core.Element;
using AssetManagementTest.PageObjects;
using OpenQA.Selenium;
    
class HomePage : BasePage
{
    private WebObject _userNameInput = new WebObject(By.Id("Username"), "UserNameInput");
    private WebObject _passwordInput = new WebObject(By.Id("Password"), "PasswordInput");
    private WebObject _loginBtn = new WebObject(By.XPath("//button[@type='submit']"), "LoginBtn");
    private WebObject _loginErrorMessage = new WebObject(By.CssSelector("div[ng-show = 'isError']"), "LoginErrorMessage");
    private WebObject _navigateBarTitle = new WebObject(By.XPath("//header[@class='header']//h1"), "NavigateBarTitle");
    public HomePage() : base()
    {
    }
    
}