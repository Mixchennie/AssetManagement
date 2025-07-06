using AssetManagement.Component;
using AssetManagementTest.Constants;
using AssetManagementTest.DataObject;
using AssetManagementTest.Model;
using AssetManagementTest.PageObjects;
using AssetManagementTest.PageObjects.Assignment;
using AssetManagementTest.PageObjects.User;

namespace AssetManagementTest.Tests
{
    public class CreateAssignmentPageTest : BaseTest
    {
        private CreateNewAssignmentPage _createAssignmentPage;
        private LoginPage _loginPage;

        public CreateAssignmentPageTest()
        {
            _createAssignmentPage = new CreateNewAssignmentPage();
            _loginPage = new LoginPage();
        }

        [Test, TestCaseSource(typeof(CreateAssignmentDataModel), nameof(CreateAssignmentDataModel.GetValidAssignmentData))]
        public void CreateAssignmentTest(CreateAssignmentDataDto data)
        {
            LoginAsAdmin();

            _createAssignmentPage.GoToCreateAssignmentForm();
            _createAssignmentPage.SelectUser(data.UserFullName);
            _createAssignmentPage.SelectAsset(data.AssetName);
            _createAssignmentPage.SetAssignedDate(data.AssignedDate);
            _createAssignmentPage.EnterNote(data.Note);
            _createAssignmentPage.ClickSaveButton();

            NotificationHandlerComponent.HandleToastMessage("Assignment created successfully");
            _createAssignmentPage.SearchAssignment(data.AssetName);
            Assert.IsTrue(_createAssignmentPage.IsAssignmentDisplayed(
                data.AssetName, data.AssignedDate
            ), "The assignment that was just created does not appear in the assignment list!");
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
}   