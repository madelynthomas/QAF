using NUnit.Framework;
using QAF.Framework;
using QAF.PageObjects;

namespace QAF.Tests
{
    [TestFixture(DriverType.Chrome)]
    class Google : BaseTest
    {
        public Google(DriverType driverType) : base(driverType)
        {
        }

        [Test]
        [TestCase]
        public void VerifyWeCanSearch()
        {
            GooglePage googlePage = new GooglePage();
            Assert.True(googlePage.CanWeSearch(), "Asserting Google Search works as intended");
        }
    }
}
