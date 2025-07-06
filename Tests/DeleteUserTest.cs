using AssetManagement.Component;
using AssetManagementTest.Constants;
using AssetManagementTest.DataObject;
using AssetManagementTest.Model;
using AssetManagementTest.PageObjects;
using AssetManagementTest.PageObjects.User;

namespace AssetManagementTest.Tests;

public class DeleteUserTest : BaseTest
{
    private LoginPage _loginPage;
    private CreateNewUserPage _createNewUserPage;
    private ManageUserPage _manageUserPage;

    public DeleteUserTest()
    {
        _loginPage = new LoginPage();
        _createNewUserPage = new CreateNewUserPage();
        _manageUserPage = new ManageUserPage();
    }

    [Test, TestCaseSource(typeof(DeleteUserDataModel), nameof(DeleteUserDataModel.GetDeleteUserData))]
    public void DeleteUserSuccessfullyTest(DeleteUserDataDto userData)
    {
        LoginAsAdmin();
        _manageUserPage.GoToUserManagement();
        _manageUserPage.GoToCreateUserForm();
        _createNewUserPage.FillUserForm(
            new CreateUserDataDto()
            {
                Email = userData.Email,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Gender = userData.Gender,
                DateOfBirth = userData.DateOfBirth,
                JoinedDate = userData.JoinedDate,
                Type = userData.Type
            }
        );
        _createNewUserPage.ClickSaveButton();

        var userFullName = $"{userData.FirstName} {userData.LastName}";
        _manageUserPage.WaitForLoadingSpinnerToDisappear();
        _manageUserPage.SearchUser(userFullName);
        _manageUserPage.DisableUser(userFullName, userData.JoinedDate, userData.Type);
        _manageUserPage.WaitForLoadingSpinnerToDisappear();
        _manageUserPage.SearchUserWithWait(userFullName);
        _manageUserPage.WaitForLoadingSpinnerToDisappear();
        Assert.True(
            _manageUserPage.IsNoResultsFoundDisplayed(),
            "The newly deleted user does not appear on the user list!"
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
