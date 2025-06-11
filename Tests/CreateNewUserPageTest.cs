using AssetManagement.Component;
using AssetManagementTest.Constants;
using AssetManagementTest.DataObject;
using AssetManagementTest.Model;
using AssetManagementTest.PageObjects;
using AssetManagementTest.PageObjects.User;

namespace AssetManagementTest.Tests;
public class CreateNewUserPageTest : BaseTest
{
    private LoginPage _loginPage;
    private CreateNewUserPage _createNewUserPage;

    public CreateNewUserPageTest()
    {
        _loginPage = new LoginPage();
        _createNewUserPage = new CreateNewUserPage();
    }

    [Test, TestCaseSource(typeof(CreateUserDataModel), nameof(CreateUserDataModel.GetAdminUserData))]
    public void CreateAdminUserTest(CreateUserDataDto userData)
    {
        LoginAsAdmin();
        _createNewUserPage.GoToCreateUserForm();
        _createNewUserPage.FillUserForm(userData);
        _createNewUserPage.ClickSaveButton();

        NotificationHandlerComponent.HandleToastMessage(MessageConstants.CreateNewUserSuccess);
        _createNewUserPage.SearchUser(userData.FirstName + " " + userData.LastName);
        Assert.IsTrue(
            _createNewUserPage.IsUserDisplayed(
                userData.FirstName + " " + userData.LastName,
                userData.JoinedDate,
                userData.Type
            ),
            "The newly created user does not appear on the user list!"
        );
    }

    [Test, TestCaseSource(typeof(CreateUserDataModel), nameof(CreateUserDataModel.GetStaffUserData))]
    public void CreateStaffUserTest(CreateUserDataDto userData)
    {
        LoginAsAdmin();
        _createNewUserPage.GoToCreateUserForm();
        _createNewUserPage.FillUserForm(userData);
        _createNewUserPage.ClickSaveButton();
        NotificationHandlerComponent.HandleToastMessage(MessageConstants.CreateNewUserSuccess);
        _createNewUserPage.SearchUser(userData.FirstName + " " + userData.LastName);
        Assert.IsTrue(
            _createNewUserPage.IsUserDisplayed(
                userData.FirstName + " " + userData.LastName,
                userData.JoinedDate,
                userData.Type
            ),
            "The user that was just created does not appear in the user list!"
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
