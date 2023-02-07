using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System.Reflection;

namespace Automation.Common;

/// <summary>
/// Holds IWebDriver objects extensions.
/// </summary>
public static class DriverUtilities
{
    private static string? _mainWindowHandle;

    /// <summary>
    /// Creates and returns a WebDriver instance.
    /// </summary>
    /// <param name="browser">The browser type. Options are "chrome", "firefox", "edge".</param>
    /// <param name="optionArgs">A string array of arguments to initialize the browser with.</param>
    /// <param name="timeoutInSeconds">The implicit timeout in seconds.</param>
    /// <param name="maximize">Starts the browser maximized.</param>
    /// <returns>A WebDriver instance.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid browser type is provided.</exception>
    public static IWebDriver NewWebDriverInstance(string browser, string[] optionArgs, bool maximize = true)
    {
        IWebDriver driver;
        switch(browser)
        {
            case "chrome":
                var optionsCh = new ChromeOptions();
                optionsCh.AddArguments(optionArgs);
                driver = new ChromeDriver(optionsCh);
                break;
            case "firefox":
                var optionsFF = new FirefoxOptions();
                optionsFF.AddArguments(optionArgs);
                driver = new FirefoxDriver(optionsFF);
                break;
            case "edge":
                var optionsE = new EdgeOptions();
                optionsE.AddArguments(optionArgs);
                driver = new EdgeDriver(optionsE);
                break;
            default: throw new ArgumentException($"The specified browser type '{browser}' is not supported. Select 'chrome', 'firefox', or 'edge'");
        }
        if(maximize) driver.Maximize();        
        _mainWindowHandle = driver.CurrentWindowHandle;
        return driver;
    }

    /// <summary>
    /// Maximize the driver's instance window.
    /// </summary>
    /// <param name="driver"></param>
    public static void Maximize(this IWebDriver driver)
    {
        driver.Manage().Window.Maximize();
        Console.WriteLine("Driver window maximized.");
    }

    /// <summary>
    /// Closes the driver instance and disposes the object.
    /// </summary>
    /// <param name="driver"></param>
    public static void Shutdown(this IWebDriver driver)
    {
        driver.Close();
        driver.Dispose();
        Console.WriteLine($"Driver instance disposed.");
    }

    /// <summary>
    /// Change the driver's window size to a specified size in pixels.
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void Resize(this IWebDriver driver, int width, int height)
    {
        driver.Manage().Window.Size = new System.Drawing.Size(width, height);
        Console.WriteLine($"Browser window size set to {width}x{height}");
    }

    /// <summary>
    /// Switch to the latest window opened by the driver.
    /// </summary>
    /// <param name="driver"></param>
    /// <exception cref="InvalidOperationException">Thrown when there's no new driver window to switch to.</exception>
    public static void SwitchToNewWindow(this IWebDriver driver)
    {
        var lastWindowHandle = driver.WindowHandles.TakeLast(1).SingleOrDefault();
        if(lastWindowHandle == _mainWindowHandle)
        {
            throw new InvalidOperationException("There are no windows or tabs for the driver to switch to.");
        }
        driver.SwitchTo().Window(lastWindowHandle);
        Console.WriteLine("Switched to new window...");
    }

    /// <summary>
    /// Switch to the initial driver window.
    /// </summary>
    /// <param name="driver"></param>
    public static void SwitchToMainWindow(this IWebDriver driver)
    {
        driver.Close();
        driver.SwitchTo().Window(_mainWindowHandle);
        Console.WriteLine("Switched to main window...");
    }

    /// <summary>
    /// Highlights an element using JavaScript by painting a coloured border around it.
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="element">An existing web element.</param>
    /// <param name="color">The border color. Defaults to 'red'. Use available css colors.</param>
    public static void HighlightElement(this IWebDriver driver, IWebElement element, string color = "red")
    {
        var jsDriver = (IJavaScriptExecutor)driver;
        string highlightJavascript = $"arguments[0].style.cssText = \"border-width: 3px; border-style: solid; border-color: {color}\";";
        jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
    }

    /// <summary>
    /// Highlights an element using JavaScript by painting a coloured border around it.
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="element">An existing select element.</param>
    /// <param name="color">The border color. Defaults to 'red'. Use available css colors.</param>
    public static void HighlightElement(this IWebDriver driver, SelectElement element, string color)
    {
        var jsDriver = (IJavaScriptExecutor)driver;
        string highlightJavascript = $"arguments[0].style.cssText = \"border-width: 3px; border-style: solid; border-color: {color}\";";
        jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
    }

    /// <summary>
    /// Captures a screenshot and saves it to the specified directory.
    /// </summary>
    /// <param name="driver">Current driver instance.</param>
    /// <param name="directory">Target directory. Defaults to the root folder of the assembly.</param>
    /// <param name="fileName">File name. Defaults to "TestName_HH-mm-ss.png".</param>
    /// <returns>Returns the full file path of the screenshot.</returns>
    public static string CaptureScreenshot(this IWebDriver driver, string directory = "", string fileName = "")
    {
        if (directory == "")
        {
            directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Run_" + DateTime.Now.ToString("yyMMdd"));
        }

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        if (fileName == "")
        {
            fileName = $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:HH-mm-ss}.png";
        }
        var filePath = $"{directory}\\{fileName}";

        var ss = driver.TakeScreenshot();

        ss.SaveAsFile(filePath, ScreenshotImageFormat.Png);
        Console.WriteLine($"Screenshot saved at {filePath}");

        return filePath;
    }

    /// <summary>
    /// Captures a screenshot and returns it as a base64 encoded string.
    /// </summary>
    /// <param name="driver">The driver instance.</param>
    /// <returns>A Base64 string.</returns>
    public static string CaptureScreenshotAsBase64String(this IWebDriver driver)
    {
        var ss = driver.TakeScreenshot().AsBase64EncodedString;
        Console.WriteLine("Screenshot captured.");
        return ss;
    }

}