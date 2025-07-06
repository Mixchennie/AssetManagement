using System.Globalization;
using AssetManagement.Component;
using AssetManagementTest.Constants;
using AssetManagementTest.DataObject;
using AssetManagementTest.Model;
using AssetManagementTest.PageObjects;
using AssetManagementTest.PageObjects.User;

namespace AssetManagementTest.Tests;

public class EditUserPageTest : BaseTest
{
    private LoginPage _loginPage;
    private EditUserPage _editUserPage;
    private ManageUserPage _manageUserPage;

    private CreateNewUserPage _createNewUserPage;

    public EditUserPageTest()
    {
        _loginPage = new LoginPage();
        _editUserPage = new EditUserPage();
        _manageUserPage = new ManageUserPage();
        _createNewUserPage = new CreateNewUserPage();
    }

    [Test, TestCaseSource(typeof(EditUserDataModel), nameof(EditUserDataModel.GetEditUserWithValidData))]
    public void EditUserWithValidDataTest(EditUserDataDto userData)
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
        _manageUserPage.WaitForLoadingSpinnerToDisappear();
        _editUserPage.ClickEditUserByInfo(userFullName, userData.JoinedDate, userData.Type);

        userData.JoinedDate = IncreaseDay(userData.JoinedDate);
        userData.DateOfBirth = IncreaseDay(userData.DateOfBirth);
        userData.Gender = userData.Gender == "MALE" ? "FEMALE" : "MALE";
        userData.Type = userData.Type == "Staff" ? "Admin" : "Staff";
        _editUserPage.FillUserForm(userData);
        _editUserPage.ClickSaveButton();

        NotificationHandlerComponent.HandleToastMessage(MessageConstants.EditUserSuccess);
        _manageUserPage.WaitForLoadingSpinnerToDisappear();
        _manageUserPage.SearchUser(userFullName);
        Assert.IsTrue(
            _editUserPage.IsUserDisplayed(
                userFullName,
                userData.JoinedDate,
                userData.Type
            ),
            "The newly edited user does not appear on the user list!"
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

    private string IncreaseDay(string dateStr)
    {
        DateTime dt;
        if (!DateTime.TryParseExact(dateStr, new[] { "yyyy-MM-dd", "d MMMM yyyy" },
            CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        {
            dt = DateTime.Parse(dateStr, CultureInfo.InvariantCulture);
        }
        return dt.AddDays(1).ToString("d MMMM yyyy", CultureInfo.InvariantCulture);
    }
}
