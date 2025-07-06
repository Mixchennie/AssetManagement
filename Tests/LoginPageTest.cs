using AssetManagementTest.Component;
using AssetManagementTest.Constants;
using AssetManagementTest.DataObject;
using AssetManagementTest.Model;
using AssetManagementTest.PageObjects;
using AssetManagementTest.Report;

namespace AssetManagementTest.Tests;

public class LoginPageTest : BaseTest
{
    private readonly LoginPage _loginPage;
    private readonly NavigationComponent _navigationComponent;

    public LoginPageTest()
    {
        _loginPage = new LoginPage();
        _navigationComponent = new NavigationComponent();
    }

    [Test, TestCaseSource(typeof(LoginDataModel), nameof(LoginDataModel.GetValidLoginTestData))]
    public void LoginWithValidUsernameAndPasswordTest(LoginDataDto loginDataDto)
    {
        ExtentReportHelper.LogTestStep($"Login with valid username: {loginDataDto.Username} and password: {loginDataDto.Password}");
        _loginPage.DoLogin(loginDataDto.Username, loginDataDto.Password);

        Assert.Multiple(() =>
            {   
                ExtentReportHelper.LogTestStep($"Verify login success message: {MessageConstants.LoginSuccess}");
                _loginPage.VerifyLoginSuccessMessage(MessageConstants.LoginSuccess);
                ExtentReportHelper.LogTestStep("Verify navigate bar title after login with valid username and password");
                Assert.That(_navigationComponent.GetNavigateBarTitleText(), Is.EqualTo(NavigationTitleConstant.Home),
                    "Navigate bar title is not displayed exactly after login");
            });
    }

    [Test, TestCaseSource(typeof(LoginDataModel), nameof(LoginDataModel.GetInvalidLoginUsernameTestData))]
    public void LoginWithInvalidUsernameTest(LoginDataDto loginDataDto)
    {

        ExtentReportHelper.LogTestStep($"Login with invalid username: {loginDataDto.Username} and password: {loginDataDto.Password}");
        _loginPage.DoLogin(loginDataDto.Username, loginDataDto.Password);

        
        Assert.Multiple(() =>
            {
                ExtentReportHelper.LogTestStep($"Verify login error message: {MessageConstants.AuthenticationFailedInvalidUsername}");
                _loginPage.VerifyLoginSuccessMessage(MessageConstants.AuthenticationFailedInvalidUsername);
                ExtentReportHelper.LogTestStep("Verify navigate bar title after login with invalid username");
                Assert.That(_navigationComponent.GetNavigateBarTitleText(), Is.EqualTo(NavigationTitleConstant.OnlineAssetManagement),
                    "Navigate bar title is not displayed exactly after login");
            });
    }

    [Test, TestCaseSource(typeof(LoginDataModel), nameof(LoginDataModel.GetInvalidLoginPasswordTestData))]
    public void LoginWithInvalidPasswordTest(LoginDataDto loginDataDto)
    {
        ExtentReportHelper.LogTestStep($"Login with username: {loginDataDto.Username} and invalid password: {loginDataDto.Password}");
        _loginPage.DoLogin(loginDataDto.Username, loginDataDto.Password);

        Assert.Multiple(() =>
           {
               ExtentReportHelper.LogTestStep($"Verify login error message: {MessageConstants.AuthenticationFailedInvalidPassword}");
               _loginPage.VerifyLoginSuccessMessage(MessageConstants.AuthenticationFailedInvalidPassword);
               ExtentReportHelper.LogTestStep("Verify navigate bar title after login with invalid password");
               Assert.That(_navigationComponent.GetNavigateBarTitleText(), Is.EqualTo(NavigationTitleConstant.OnlineAssetManagement),
                   "Navigate bar title is not displayed exactly after login");
           });
    }
}