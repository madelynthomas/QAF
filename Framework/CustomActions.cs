using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace QAF.Framework
{
    class CustomActions
    {
        private static readonly string
            screenshotsFolderPath = @"C:\Code\Screenshots\QAF\",
            screenshotsToBeDeleted = "ScreenshotsCleanUpDate";

        public static string GetValueFromXMLFileByAttribute(string fileLocation, string attributeId)
        {
            string value = null;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileLocation);

                XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("//attribute[@id='" + attributeId + "']");
                value = xmlElement.GetAttribute("value");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught opening XML file: " + e.ToString());
            }

            return value;
        }

        public static void UpdateValueFromXMLFileByAttribute(string fileLocation, string attributeId)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileLocation);

                XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("//attribute[@id='" + attributeId + "']");
                xmlElement.SetAttribute("value", DateTime.Today.ToString());
                xmlDocument.Save(fileLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught when opening and saving XML file: " + e.ToString());
            }
        }

        public static bool VerifyThatHttpRequestReturnsSuccess(string URLName)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URLName); ;
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            HttpStatusCode httpStatusCode = httpWebResponse.StatusCode;

            Console.WriteLine($"Http response received for URL: {URLName}\nStatus = {httpStatusCode}");

            httpWebResponse.Close();
            streamReader.Close();

            return httpStatusCode == HttpStatusCode.OK;
        }

        private static void SaveImage(string path)
        {
            Screenshot screenshot = ((ITakesScreenshot)DriverFactory.GetDriver()).GetScreenshot();

            using (MemoryStream memoryStream = new MemoryStream(screenshot.AsByteArray))

            using (Image screenShotImage = Image.FromStream(memoryStream))
            {
                Bitmap image = new Bitmap(screenShotImage);
                image.Save(path);
                image.Dispose();
            }
        }

        public static void TakeScreenshot()
        {
            try
            {
                string ScreenshotTitle = TestContext.CurrentContext.Test.Name + "_" + DriverFactory.GetDriver().GetType().ToString().Substring(23) + "_" + DateTime.Now.ToString("h: mm: ss tt") + "_" + TestContext.CurrentContext.Result.Outcome.Status;
                char[] chars = ScreenshotTitle.ToCharArray();

                for (int i = 0; i < ScreenshotTitle.Length; i++)
                {
                    if (chars[i] == '.' || chars[i] == ':' || chars[i] == ' ')
                    {
                        chars[i] = '_';
                    }
                }

                ScreenshotTitle = new string(chars);

                string finalPath = null;

                try
                {
                    finalPath = screenshotsFolderPath + ScreenshotTitle + ".png";
                    SaveImage(finalPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to save screenshot to: {screenshotsFolderPath}\n" + e);
                }

                Console.WriteLine($"Screenshot saved: {finalPath}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Failed to save screenshot to: {screenshotsFolderPath}");
            }
        }

        public static void DeleteOldScreenshots()
        {
            string screenshotsFolder = null;

            DateTime screenshotsDeletedLastDate;

            DirectoryInfo fileDirectory = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory);
            string filePath = fileDirectory.Parent.Parent.FullName + "\\Variables.xml";

            screenshotsDeletedLastDate = DateTime.Parse(GetValueFromXMLFileByAttribute(filePath, screenshotsToBeDeleted));
            {
                try
                {
                    if (screenshotsDeletedLastDate < DateTime.Today)
                    {
                        screenshotsFolder = screenshotsFolderPath;
                        Directory.GetFiles(screenshotsFolder)
                            .Select(f => new FileInfo(f))
                            .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-7))
                            .ToList()
                            .ForEach(f => f.Delete());
                        UpdateValueFromXMLFileByAttribute(filePath, screenshotsToBeDeleted);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to delete old screenshots at: {screenshotsFolderPath}\n" + e);
                }

                Console.WriteLine($"Older screenshots deleted from: {screenshotsFolder}.");
            }
        }
    }
}
