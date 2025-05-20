using SeleniumPractice.PageObjects;
using SeleniumPractice.TestData.DTO;
using SeleniumPractice.Tests;
using SeleniumPractice.Utils;

namespace SeleniumPractice.Tests
{
    class RegistrationFormTest : BaseTest
    {
        [Test, TestCaseSource(typeof(TestDataHelper), nameof(TestDataHelper.GetValidRegistrationFormTestData))]
        [Category("RegistrationFormTest")]
        [Description("Verify registering a user with all fields successfully")]
        public void RegisterUserWithAllFieldsSuccessfully(RegistrationFormDTO registrationFormData)
        {
            ExtentReportHelper.LogTestStep("Go to registration form page");
            DriverUtils.GoToUrl(ConfigurationUtils.GetConfigurationByKey("TestUrl"));

            var registrationFormPage = new RegistrationFormPage();

            ExtentReportHelper.LogTestStep("Fill registration form");
            registrationFormPage.FillRegistrationForm(registrationFormData);

            ExtentReportHelper.LogTestStep("Verify thank for submiting title");
            Assert.That(registrationFormPage.GetThankForSubmitingTitle(), Is.EqualTo("Thanks for submitting the form"),
            "The thank for submiting title is not displayed exactly");

            ExtentReportHelper.LogTestStep("Verify all titles contain keyword");
            Assert.Multiple(() => {
                registrationFormPage.VerifyAllTitlesContainKeyword(registrationFormData);
            });
        }
        
        [Test, TestCaseSource(typeof(TestDataHelper), nameof(TestDataHelper.GetInvalidRegistrationFormTestData))]
        [Category("RegistrationFormTest")]
        [Description("Verify registering a user with mandatory fields successfully")]
        public void RegisterUserWithMandatoryFieldsSuccessfully(RegistrationFormDTO registrationFormData)
        {
            ExtentReportHelper.LogTestStep("Go to registration form page");
            DriverUtils.GoToUrl(ConfigurationUtils.GetConfigurationByKey("TestUrl"));
            
            var registrationFormPage = new RegistrationFormPage();

            ExtentReportHelper.LogTestStep("Enter to mandatory fields");
            registrationFormPage.EnterToMandatoryFields(registrationFormData);

            ExtentReportHelper.LogTestStep("Verify thank for submiting title");
            Assert.That(registrationFormPage.GetThankForSubmitingTitle(), Is.EqualTo("Thanks for submitting the form"),
            "The thank for submiting title is not displayed exactly");

            ExtentReportHelper.LogTestStep("Verify all titles contain keyword");
            Assert.Multiple(() =>
            {
                registrationFormPage.VerifyAllTitlesContainKeyword(registrationFormData);
            });
        }
    }
}