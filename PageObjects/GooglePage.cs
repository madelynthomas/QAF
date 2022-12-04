using OpenQA.Selenium;
using QAF.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace QAF.PageObjects
{
    public class GooglePage : BasePage
    {
        public GooglePage() : base()
        {
            if (!driver.Url.Contains("google"))
            {
                NavigateToUrl();
                wait.Until(ExpectedConditions.UrlContains(Url));
            }
        }

        public bool CanWeSearch()
        {
            try
            {
                IWebElement searchBar = driver.FindElement(By.XPath(Selectors["searchInput"]));
                searchBar.SendKeys("Hello, World!");
                Thread.Sleep(3000);

                IWebElement searchButton = driver.FindElement(By.XPath(Selectors["searchButton"]));
                searchButton.Click();
                Thread.Sleep(5000);

                return true;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Unable to locate element: " + e);
                
                return false;
            }
        }

        public override Dictionary<string, string> DevSelectors()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, string> QaSelectors()
        {
            return new Dictionary<string, string>()
            {
                { "searchInput", "/html/body/div/div[3]/form/div[2]/div[1]/div[1]/div/div[2]/input" },
                { "searchButton", "/html/body/div/div[3]/form/div[2]/div[1]/div[2]/div[2]/div[2]/center/input[1]" }
            };
        }

        public override Dictionary<string, string> UatSelectors()
        {
            throw new NotImplementedException();
        }

        public override PageUrl GetPageUrl()
        {
            return new PageUrl()
            {
                Dev = "",
                Qa = "https://www.google.com",
                Uat = ""
            };
        }
    }
}
