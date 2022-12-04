using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.IO;

namespace QAF.Framework
{
    public class DriverFactory
    {
        static IWebDriver driver;

        public static IWebDriver GetDriver()
        {
            return driver;
        }

        private static bool DriverInstallationCheck(DriverType driverType)
        {
            string location;

            switch (driverType)
            {
                case DriverType.Chrome:
                    location = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                    break;
                case DriverType.Firefox:
                    location = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                    break;
                case DriverType.InternetExplorer:
                    location = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe";
                    break;
                default:
                    Console.WriteLine("Driver was unable to be located on filestystem");
                    return false;
            }

            return File.Exists(location);
        }

        public static void SetUpDriver(DriverType driverType)
        {
            switch (driverType)
            {
                case DriverType.Chrome:
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    chromeOptions.AddArguments("--no-sandbox");

                    if (DriverInstallationCheck(DriverType.Chrome))
                    {
                        driver = new ChromeDriver(chromeOptions);
                    }
                    else
                    {
                        Console.WriteLine(@"Chrome was not found at: C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
                    }

                    break;
                case DriverType.Firefox:
                    FirefoxOptions firefoxOptions = new FirefoxOptions();

                    if (DriverInstallationCheck(DriverType.Firefox))
                    {
                        driver = new FirefoxDriver(firefoxOptions);
                    }
                    else
                    {
                        Console.WriteLine(@"Firefox was not found at: C:\Program Files(x86)\Mozilla Firefox\firefox.exe");
                    }

                    break;
                case DriverType.InternetExplorer:
                    InternetExplorerOptions internetExplorerOptions = new InternetExplorerOptions
                    {
                        InitialBrowserUrl = "https://www.google.com",
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        IgnoreZoomLevel = true,
                        EnableNativeEvents = true
                    };

                    if (DriverInstallationCheck(DriverType.InternetExplorer))
                    {
                        driver = new InternetExplorerDriver(internetExplorerOptions);
                        driver.Navigate().GoToUrl(internetExplorerOptions.InitialBrowserUrl);
                    }
                    else
                    {
                        Console.WriteLine(@"Internet Explorer was not found at: C:\Program Files (x86)\Internet Explorer\iexplore.exe");
                    }

                    break;
                default:
                    Console.WriteLine("No driver specified");
                    break;
            }

            driver.Manage().Window.Maximize();
        }
    }
}
