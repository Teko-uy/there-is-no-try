using NUnit.Framework;
using OpenQA.Selenium;
using Automation.Common;
using NUnit.Framework.Interfaces;
using System.Text;

namespace Hms.Essette.GUI.TestsNew;

/// <summary>
/// Base class for all test fixtures implemented.
/// </summary>
[TestFixture]
public class BaseTest
{
    private IWebDriver _driver;

    protected IWebDriver Driver { get { return _driver; } }
    
    /// <summary>
    /// Runs once before starting any test run.
    /// </summary>
    [OneTimeSetUp]
    protected virtual void OneTimeSetUp()
    {
        TestContext.WriteLine("Starting test run...");
        _driver = DriverUtilities.NewWebDriverInstance("chrome", new string[] { "--window-size=1920,1080", "--no-sandbox"/*, "--headless" */});
        _driver.Maximize();
        _driver.Navigate().GoToUrl("https://www.mercadolibre.com.uy/");
    }

    /// <summary>
    /// Runs once after every test run finished.
    /// </summary>
    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        _driver.Shutdown();
        TestContext.WriteLine("End test run.");
        //Report.Save();
    }

    /// <summary>
    /// Runs before each test runs.
    /// </summary>
    [SetUp]
    protected virtual void SetUp()
    {
        Report.NewTest(TestContext.CurrentContext.Test.Name);
    }

    /// <summary>
    /// Runs after each test runs.
    /// </summary>
    [TearDown]
    protected virtual void TearDown()
    {

    }

    /// <summary>
    /// Wraps the test in an execution block and captures any errors generated during the execution.
    /// </summary>
    /// <param name="test">Test action methods.</param>
    protected virtual void RunTest(Action test)
    {
        try
        {
            test();
            if (TestContext.CurrentContext.Result.Assertions.Any(a => a.Status == AssertionStatus.Failed))
            {
                Report.FailAction("Test failed: " + TestContext.CurrentContext.Result.Message, Driver.CaptureScreenshotAsBase64String());
            }
            else
            {
                Report.PassAction("Test passed!", Driver.CaptureScreenshotAsBase64String());
            }
        }
        catch (Exception ex)
        {
            Report.FailAction("An unexpected error ocurred.");
            Report.FailAction(ex.Message + Environment.NewLine + ex, Driver.CaptureScreenshotAsBase64String());
            throw;
        }
    }

    /// <summary>
    /// Saves the product details to a txt file. If no save location is provided, the file will be saved on the desktop.
    /// </summary>
    /// <param name="details"></param>
    /// <param name="saveLocation"></param>
    public void SaveProductDetails(List<string> details, string saveLocation = "")
    {
        string fileName = $"Camisetas_ML_{DateTime.Now:dd-MM-yyyy}.csv";
        string filePath;
        if (!string.IsNullOrEmpty(saveLocation))
        {
            filePath = Path.Combine(saveLocation, fileName);
        }
        else
        {
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        }
        File.WriteAllLines(filePath, details, Encoding.UTF8);
    }
}