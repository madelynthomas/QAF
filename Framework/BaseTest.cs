using NUnit.Framework;
using System;
using System.Configuration;

namespace QAF.Framework
{
    class BaseTest
    {
        private readonly DriverType driverType;

        public BaseTest(DriverType driverType)
        {
            this.driverType = driverType;
        }

        [OneTimeSetUp]
        public void RunBeforeTextFixture()
        {
            CustomActions.DeleteOldScreenshots();
        }

        [SetUp]
        public void BaseSetUp()
        {
            try
            {
                Console.WriteLine("========== START TEST SET UP ==========");
                Console.WriteLine(@"Environment: " + ConfigurationManager.AppSettings.Get("Environment").ToUpper() + "\nBrowser: " + driverType);
                
                DriverFactory.SetUpDriver(driverType);

                Console.WriteLine("========== TEST SET UP IS COMPLETE ==========");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured during BaseSetUp: " + e);
            }
        }

        [TearDown]
        public void BaseTearDown()
        {
            try
            {
                Console.WriteLine("========== START TEST TEAR DOWN ==========");

                CustomActions.TakeScreenshot();

                KillDriver();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured during BaseTearDown: " + e);
            }
        }

        public void KillDriver()
        {
            DriverFactory.GetDriver()?.Close();
            DriverFactory.GetDriver()?.Quit();
        }
    }
}
