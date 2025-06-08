using AssetManagementTest.Core.Element;
using AssetManagementTest.PageObjects;
using OpenQA.Selenium;
    
class CreateNewUserPage : BasePage
{
    private WebObject _firstNameInput = new WebObject(By.Id("firstName"), "firstNameInput");
    private WebObject _lastNameInput = new WebObject(By.Id("lastName"), "lastNameInput");
    private WebObject _dateOfBirthDatePicker = new WebObject(By.XPath("//button[@type='submit']"), "LoginBtn");
    private WebObject _femaleRadioBtn = new WebObject(By.XPath("//input[@value='FEMALE']"), "FemaleRadioBtn");
    private WebObject _maleRadioBtn = new WebObject(By.XPath("//input[@value='MALE']"), "MaleRadioBtn");
    private WebObject _loginErrorMessage = new WebObject(By.CssSelector("div[ng-show = 'isError']"), "LoginErrorMessage");
    private WebObject _navigateBarTitle = new WebObject(By.XPath("//header[@class='header']//h1"), "NavigateBarTitle");
    public CreateNewUserPage() : base()
    {
    }
    
}