using AssetManagement.Component;
using AssetManagementTest.Constants;
using AssetManagementTest.DataObject;
using AssetManagementTest.Model;
using AssetManagementTest.PageObjects;
using AssetManagementTest.PageObjects.RequestForReturn;
using AssetManagementTest.PageObjects.User;

namespace AssetManagementTest.Tests;

public class RequestForReturnPageTest : BaseTest
{
    private LoginPage _loginPage;
    private CreateNewUserPage _createNewUserPage;
    private ManageUserPage _manageUserPage;

    private ManageRequestForReturnPage _requestForReturnPage;
    public RequestForReturnPageTest()
    {
        _loginPage = new LoginPage();
        _createNewUserPage = new CreateNewUserPage();
        _manageUserPage = new ManageUserPage();
        _requestForReturnPage = new ManageRequestForReturnPage();
    }

    [Test]
    public void RequestForReturnPageSuccessfullyTest()
    {
        LoginAsAdmin();
        _requestForReturnPage.GoToRequestForReturnManagement();
        var assetCode = _requestForReturnPage.ConfirmRequestForReturn();
        if (assetCode == null)
        {
            Assert.Fail("No request for return found to confirm.");
        }
        _requestForReturnPage.WaitForLoadingSpinnerToDisappear();
        _requestForReturnPage.SearchAsset(assetCode);
        _requestForReturnPage.WaitForLoadingSpinnerToDisappear();
        Assert.True(
            _requestForReturnPage.IsNoAssetFoundWithWaitForReturnDisplayed(assetCode),
            "No asset with wait for return displayed!"
        );
    }
    private void LoginAsAdmin()
    {
        var admin = new LoginDataDto
        {
            Username = "admindnuo",
            Password = "Admindnuo@010119901"
        };
        _loginPage.DoLogin(admin.Username, admin.Password);
        _loginPage.VerifyLoginSuccessMessage(MessageConstants.LoginSuccess);
    }


}
