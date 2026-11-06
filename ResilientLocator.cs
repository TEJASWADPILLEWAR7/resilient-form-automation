using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace ResilientFormAutomation
{
    public class ResilientLocator
    {
        private readonly IWebDriver _driver;

        public ResilientLocator(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement FindTextBox(string fieldName)
        {
            var elements = _driver.FindElements(By.CssSelector("input[type='text'], input:not([type])"));
            return GetBestMatch(elements, fieldName);
        }

        public IWebElement FindRadioButton(string optionName)
        {
            var elements = _driver.FindElements(By.CssSelector("input[type='radio']"));
            return GetBestMatch(elements, optionName);
        }

        public IWebElement FindDropdown(string fieldName)
        {
            var elements = _driver.FindElements(By.TagName("select"));
            return GetBestMatch(elements, fieldName);
        }

        private bool ContainsWord(string? text, string target)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return Regex.IsMatch(text, $@"\b{Regex.Escape(target)}\b", RegexOptions.IgnoreCase);
        }

        private IWebElement GetBestMatch(IReadOnlyCollection<IWebElement> candidates, string targetName)
        {
            IWebElement? bestElement = null;
            int highestScore = 0;
            bool isAmbiguous = false;

            foreach (var element in candidates)
            {
                int score = 0;

                string? id = element.GetAttribute("id");
                if (!string.IsNullOrEmpty(id))
                {
                    var labels = _driver.FindElements(By.CssSelector($"label[for='{id}']"));
                    foreach (var label in labels)
                    {
                        if (ContainsWord(label.Text, targetName)) score += 50;
                    }
                }

                string? placeholder = element.GetAttribute("placeholder");
                if (ContainsWord(placeholder, targetName)) score += 30;

                string? name = element.GetAttribute("name");
                if (ContainsWord(name, targetName)) score += 20;

                string? ariaLabel = element.GetAttribute("aria-label");
                if (ContainsWord(ariaLabel, targetName)) score += 20;

                string? valueAttr = element.GetAttribute("value");
                if (ContainsWord(valueAttr, targetName)) score += 20;

                try
                {
                    string parentText = element.FindElement(By.XPath("..")).Text;
                    if (ContainsWord(parentText, targetName)) score += 15;
                }
                catch (Exception) { }

                if (score > highestScore)
                {
                    highestScore = score;
                    bestElement = element;
                    isAmbiguous = false;
                }
                else if (score == highestScore && score > 0)
                {
                    isAmbiguous = true;
                }
            }

            if (highestScore < 20 || bestElement == null)
            {
                throw new Exception($"Could not confidently identify field for '{targetName}'. Highest score was {highestScore}.");
            }

            if (isAmbiguous)
            {
                throw new Exception($"Ambiguity detected for '{targetName}'. Multiple fields share the highest confidence score of {highestScore}.");
            }

            return bestElement;
        }
    }
}