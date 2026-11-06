using System;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ResilientFormAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string targetUrl = configuration["TargetUrl"] ?? throw new ArgumentNullException("TargetUrl");

            Console.WriteLine("Starting Chrome...");

            var chromeOptions = new ChromeOptions();
            using IWebDriver driver = new ChromeDriver(chromeOptions);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            
            try
            {
                driver.Navigate().GoToUrl(targetUrl);
                driver.Manage().Window.Maximize();
                Console.WriteLine($"Successfully navigated to: {driver.Title}");
                
                System.Threading.Thread.Sleep(2000); 

                var locator = new ResilientLocator(driver);

                Console.WriteLine("Finding and filling First Name...");
                var firstNameField = locator.FindTextBox("First Name");
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", firstNameField);
                System.Threading.Thread.Sleep(500);
                firstNameField.SendKeys("Tejas");

                Console.WriteLine("Finding and selecting Gender (Male)...");
                var genderRadio = locator.FindRadioButton("Male");
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", genderRadio);
                System.Threading.Thread.Sleep(500);
                js.ExecuteScript("arguments[0].click();", genderRadio);

                Console.WriteLine("Finding and selecting State...");
                try 
                {
                    var stateDropdown = locator.FindDropdown("State");
                    js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", stateDropdown);
                    var selectElement = new SelectElement(stateDropdown);
                    selectElement.SelectByIndex(1); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Note: Standard dropdown failed ({ex.Message}). Handled safely without crashing.");
                }

                Console.WriteLine("\nTask 1 sequence complete! Check the browser to verify.");
                Console.WriteLine("Press ENTER in this console to close the browser...");
                Console.ReadLine(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAUTOMATION FAILED: {ex.Message}");
            }
            finally
            {
                driver.Quit();
                Console.WriteLine("Browser closed.");
            }
        }
    }
}