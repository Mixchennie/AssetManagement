using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace AssetManagement.Component;
public class NotificationHandlerComponent
{
    private static IWebDriver _driver => BrowserFactory.GetWebDriver();

    public static void HandleAlert(string expectedText = null, bool accept = true, int timeoutInSeconds = 10)
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

            if (!string.IsNullOrEmpty(expectedText) && !alert.Text.Contains(expectedText))
            {
                throw new Exception($"Alert text does not match expected. Expected: {expectedText}, Actual: {alert.Text}");
            }

            if (accept)
                alert.Accept();
            else
                alert.Dismiss();
        }
        catch (WebDriverTimeoutException)
        {
            throw new TimeoutException("Alert was not present within the timeout period.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while handling the alert: " + ex.Message);
        }
    }

    public static void HandleToastMessage(string expectedText, int timeoutInSeconds = 10)
    {
        try
        {
            By toastLocator = By.XPath("//div[@role='alert']");
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement toast = wait.Until(ExpectedConditions.ElementIsVisible(toastLocator));

            string actualText = toast.Text.Trim();
            if (!actualText.Contains(expectedText))
            {
                throw new InvalidOperationException(
                    $"Toast text did not match. Expected to contain: \"{expectedText}\" but was: \"{actualText}\"");
            }

            CloseToastIfExists(toast);
        }
        catch (WebDriverTimeoutException)
        {
            throw new TimeoutException($"Toast message \"{expectedText}\" did not appear within {timeoutInSeconds}s.");
        }
        catch (NoSuchElementException)
        {
            throw new NoSuchElementException("Toast message element was not found.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while handling the toast message: " + ex.Message);
        }
    }

    private static void CloseToastIfExists(IWebElement toast)
    {
        try
        {
            By closeButton = By.XPath("//div[@role='alert']//button");
            IWebElement btn = toast.FindElement(closeButton);
            btn.Click();
        }
        catch (NoSuchElementException)
        {
            // No close button found, do nothing.
        }
    }

    public static void HandleConfirmationModal(string confirmText = null, bool accept = true)
    {
        try
        {
            By modalLocator = By.XPath("//div[@role='dialog']");
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(modalLocator));

            if (!string.IsNullOrEmpty(confirmText))
            {
                string actualText = modal.Text.Trim();
                if (!actualText.Contains(confirmText))
                {
                    throw new Exception($"Confirmation modal text does not match. Expected: {confirmText}, Actual: {actualText}");
                }
            }

            By confirmButton = By.XPath("//button[text()='OK']");
            IWebElement confirmBtn = modal.FindElement(confirmButton);
            if (accept)
                confirmBtn.Click();
            else
                modal.FindElement(By.XPath("//button[text()='Cancel']")).Click();
        }
        catch (WebDriverTimeoutException)
        {
            throw new TimeoutException("Confirmation modal did not appear within the timeout period.");
        }
        catch (NoSuchElementException)
        {
            throw new NoSuchElementException("Confirmation modal or its buttons were not found.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while handling the confirmation modal: " + ex.Message);
        }
    }

    public static void HandleInputPopup(string inputText = null, bool accept = true)
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IAlert popup = wait.Until(ExpectedConditions.AlertIsPresent());

            if (inputText != null)
            {
                popup.SendKeys(inputText);
            }

            if (accept)
                popup.Accept();
            else
                popup.Dismiss();
        }
        catch (WebDriverTimeoutException)
        {
            throw new TimeoutException("Popup did not appear within the timeout period.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while handling the input popup: " + ex.Message);
        }
    }
}
