using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace QAF.Framework
{
    public abstract class BasePage
    {
        protected ILog Logger;
        protected IWebDriver driver;
        protected WebDriverWait wait;
        public string Url;
        public Dictionary<string, string> Selectors;

        protected BasePage()
        {
            driver = DriverFactory.GetDriver();

            wait = new WebDriverWait(DriverFactory.GetDriver(), new TimeSpan(0, 0, 15));

            InitializePageObject();
        }

        private void InitializePageObject()
        {
            switch (ConfigurationManager.AppSettings.Get("Environment").ToLower())
            {
                case DataObjects.Environment.Dev:
                    Url = GetPageUrl().Dev;
                    Selectors = DevSelectors();
                    break;
                case DataObjects.Environment.Qa:
                    Url = GetPageUrl().Qa;
                    Selectors = QaSelectors();
                    break;
                case DataObjects.Environment.Uat:
                    Url = GetPageUrl().Uat;
                    Selectors = UatSelectors();
                    break;
            }
        }

        public void NavigateToUrl()
        {
            if (Url != null)
            {
                driver.Navigate().GoToUrl(Url);
            }
        }

        public abstract Dictionary<string, string> DevSelectors();

        public abstract Dictionary<string, string> QaSelectors();

        public abstract Dictionary<string, string> UatSelectors();

        public abstract PageUrl GetPageUrl();
    }
}
