using System.Globalization;
using OpenQA.Selenium;
using PracticeSelenium.Pages.WebPages;
using SeleniumPractice.TestData.DTO;

namespace SeleniumPractice.PageObjects
{
    class RegistrationFormPage : BasePage
    {
        private WebObject _firstNameInput = new WebObject(By.Id("firstName"), "FirstName");
        private WebObject _lastNameInput = new WebObject(By.Id("lastName"), "LastName");
        private WebObject _emailInput = new WebObject(By.Id("userEmail"), "Email");
        private WebObject _maleRadioBtn = new WebObject(By.XPath("//input[@id='gender-radio-1']/.."), "MaleRadioBtn");
        private WebObject _femaleRadioBtn = new WebObject(By.XPath("//input[@id='gender-radio-2']/.."), "FemaleRadioBtn");
        private WebObject _otherRadioBtn = new WebObject(By.XPath("//input[@id='gender-radio-3']/.."), "OtherRadioBtn");
        private WebObject _userNumberInput = new WebObject(By.Id("userNumber"), "UserNumberInput");
        private WebObject _dateOfBirthInput = new WebObject(By.Id("dateOfBirth"), "DateOfBirthInput");
        private WebObject _subjectsInput = new WebObject(By.Id("subjectsInput"), "SubjectsInput");
        private WebObject _suggestionDropdown = new WebObject(By.CssSelector(".subjects-auto-complete__menu"), "SuggestionDropDown");
        private WebObject _sportsCheckbox = new WebObject(By.XPath("//input[@id='hobbies-checkbox-1']/.."), "SportsCheckbox");
        private WebObject _readingCheckbox = new WebObject(By.XPath("//input[@id='hobbies-checkbox-2']/.."), "ReadingCheckbox");
        private WebObject _musicCheckbox = new WebObject(By.XPath("//input[@id='hobbies-checkbox-3']/.."), "MusicCheckbox");
        private WebObject _uploadFileBtn = new WebObject(By.Id("uploadPicture"), "CurrentAddressInput");
        private WebObject _currentAddressTxtArea = new WebObject(By.Id("currentAddress"), "CurrentAddressInput");
        private WebObject _stateDropdown = new WebObject(By.XPath("//div[@id='state']//input"), "StateDropdown");
        private WebObject _cityDropdown = new WebObject(By.XPath("//div[@id='city']//input"), "CityDropdown");
        private WebObject _submitBtn = new WebObject(By.Id("submit"), "SubmitBtn");

        private WebObject _submitFormTitle = new WebObject(By.Id("example-modal-sizes-title-lg"), "SubmitFormTitle");
        private WebObject _studentNameValue = new WebObject(By.XPath("//td[text()='Student Name']/following-sibling::td"), "StudentNameValue");
        private WebObject _studentEmailValue = new WebObject(By.XPath("//td[text()='Student Email']/following-sibling::td"), "StudentEmailValue");
        private WebObject _genderValue = new WebObject(By.XPath("//td[text()='Gender']/following-sibling::td"), "GenderValue");
        private WebObject _mobileValue = new WebObject(By.XPath("//td[text()='Mobile']/following-sibling::td"), "MobileValue");
        private WebObject _dateOfBirthValue = new WebObject(By.XPath("//td[text()='Date of Birth']/following-sibling::td"), "DateOfBirthValue");
        private WebObject _subjectsValue = new WebObject(By.XPath("//td[text()='Subjects']/following-sibling::td"), "SubjectValue");
        private WebObject _hobbiesValue = new WebObject(By.XPath("//td[text()='Hobbies']/following-sibling::td"), "HobbiesValue");
        private WebObject _pictureValue = new WebObject(By.XPath("//td[text()='Picture']/following-sibling::td"), "PictureValue");
        private WebObject _addressValue = new WebObject(By.XPath("//td[text()='Address']/following-sibling::td"), "AddressValue");
        private WebObject _stateAndCityValue = new WebObject(By.XPath("//td[text()='State and City']/following-sibling::td"), "StateAndCityValue");

        public RegistrationFormPage()
        {
        }

        public void EnterFirstName(string firstName)
        {
            _firstNameInput.SendKeysToElement(firstName);
        }

        public void EnterLastName(string lastName)
        {
            _lastNameInput.SendKeysToElement(lastName);
        }

        public void EnterEmail(string email)
        {
            _emailInput.ScrollToElement();
            _emailInput.SendKeysToElement(email);
        }

        public void SelectGender(string gender)
        {
            _maleRadioBtn.ScrollToElement();
            switch (gender.ToLower())
            {
                case "male":
                    _maleRadioBtn.ClickOnElement();
                    break;
                case "female":
                    _femaleRadioBtn.ClickOnElement();
                    break;
                case "other":
                    _otherRadioBtn.ClickOnElement();
                    break;
                default:
                    throw new ArgumentException($"Invalid gender option: {gender}");
            }
        }

        public void EnterUserNumber(string userNumber)
        {
            _userNumberInput.ScrollToElement();
            _userNumberInput.SendKeysToElement(userNumber);
        }

        public void EnterDateOfBirth(string dateOfBirth)
        {
            _dateOfBirthInput.ScrollToElement();
            SelectDateWithDatePicker(_dateOfBirthInput, dateOfBirth);
        }

        public void SelectSubjects(string subjects)
        {
            string[] subjectArray = subjects.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            _subjectsInput.ScrollToElement();

            foreach (var subject in subjectArray)
            {
                _subjectsInput.SendKeysToElement(subject);
                _subjectsInput.PressEnter();
            }
        }

        public void SelectHobbies(string hobbies)
        {
            _sportsCheckbox.ScrollToElement();
            switch (hobbies.ToLower())
            {
                case "sports":
                    _sportsCheckbox.ClickOnElement();
                    break;
                case "reading":
                    _readingCheckbox.ClickOnElement();
                    break;
                case "music":
                    _musicCheckbox.ClickOnElement();
                    break;
                default:
                    throw new ArgumentException($"Invalid hobbies option: {hobbies}");
            }
        }

        public void UploadFile(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", fileName);
            _uploadFileBtn.SendKeysToElement(filePath);
        }

        public void EnterCurrentAddress(string currentAddress)
        {
            _currentAddressTxtArea.SendKeysToElement(currentAddress);
        }

        public void SelectState(string state)
        {
            _stateDropdown.ScrollToElement();
            _stateDropdown.SendKeysToElement(state);
            _stateDropdown.PressEnter();
        }

        public void SelectCity(string city)
        {
            _cityDropdown.SendKeysToElement(city);
            _cityDropdown.PressEnter();
        }

        public void ClickSubmitButton()
        {
            _submitBtn.ClickOnElement();
        }

        public void FillRegistrationForm(RegistrationFormDTO registrationFormDTO)
        {
            EnterFirstName(registrationFormDTO.FirstName ?? string.Empty);
            EnterLastName(registrationFormDTO.LastName ?? string.Empty);
            EnterEmail(registrationFormDTO.Email ?? string.Empty);
            SelectGender(registrationFormDTO.Gender ?? string.Empty);
            EnterUserNumber(registrationFormDTO.UserNumber ?? string.Empty);
            EnterDateOfBirth(registrationFormDTO.DateOfBirth ?? string.Empty);
            SelectSubjects(registrationFormDTO.Subject ?? string.Empty);
            SelectHobbies(registrationFormDTO.Hobbie ?? string.Empty);
            UploadFile(registrationFormDTO.FileName ?? string.Empty);
            EnterCurrentAddress(registrationFormDTO.CurrentAddress ?? string.Empty);
            SelectState(registrationFormDTO.State ?? string.Empty);
            SelectCity(registrationFormDTO.City ?? string.Empty);
            ClickSubmitButton();
        }

        public void EnterToMandatoryFields(RegistrationFormDTO registrationFormDTO)
        {
            EnterFirstName(registrationFormDTO.FirstName ?? string.Empty);
            EnterLastName(registrationFormDTO.LastName ?? string.Empty);
            SelectGender(registrationFormDTO.Gender ?? string.Empty);
            EnterUserNumber(registrationFormDTO.UserNumber ?? string.Empty);
            _submitBtn.ScrollToElement();
            ClickSubmitButton();
        }

        public void VerifyTitleContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            var studentName = registrationFormDTO.FirstName + " " + registrationFormDTO.LastName;
            if (studentName == " ")
                studentName = "";
            AssertTitleContainKeyword(_studentNameValue, studentName);
        }

        public void VerifyEmailContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_studentEmailValue, registrationFormDTO.Email ?? string.Empty);
        }

        public void VerifyGenderContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_genderValue, registrationFormDTO.Gender ?? string.Empty);
        }

        public void VerifyMobileContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_mobileValue, registrationFormDTO.UserNumber ?? string.Empty);
        }

        public void VerifyDateOfBirthContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_dateOfBirthValue, AddCommaToDate(registrationFormDTO.DateOfBirth ?? string.Empty));
        }

        public void VerifySubjectsContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_subjectsValue, registrationFormDTO.Subject ?? string.Empty);
        }

        public void VerifyHobbiesContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_hobbiesValue, registrationFormDTO.Hobbie ?? string.Empty);
        }

        public void VerifyPictureContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_pictureValue, registrationFormDTO.FileName ?? string.Empty);
        }

        public void VerifyAddressContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            AssertTitleContainKeyword(_addressValue, registrationFormDTO.CurrentAddress ?? string.Empty);
        }

        public void VerifyStateAndCityContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            var stateAndCity = registrationFormDTO.State + " " + registrationFormDTO.City;
            if (stateAndCity == " ")
                stateAndCity = "";
            AssertTitleContainKeyword(_stateAndCityValue, stateAndCity);
        }

        public void VerifyAllTitlesContainKeyword(RegistrationFormDTO registrationFormDTO)
        {
            VerifyTitleContainKeyword(registrationFormDTO);
            VerifyEmailContainKeyword(registrationFormDTO);
            VerifyGenderContainKeyword(registrationFormDTO);
            VerifyMobileContainKeyword(registrationFormDTO);
            VerifyDateOfBirthContainKeyword(registrationFormDTO);
            VerifySubjectsContainKeyword(registrationFormDTO);
            VerifyHobbiesContainKeyword(registrationFormDTO);
            VerifyPictureContainKeyword(registrationFormDTO);
            VerifyAddressContainKeyword(registrationFormDTO);
            VerifyStateAndCityContainKeyword(registrationFormDTO);
        }

        private static string AddCommaToDate(string inputDate)
        {
            var parts = inputDate.Split(' ');
            if (parts.Length != 3) return inputDate;

            return $"{parts[0]} {parts[1]},{parts[2]}";
        }

        public string GetThankForSubmitingTitle()
        {
            return _submitFormTitle.GetTextFromElement();
        }
    }
}