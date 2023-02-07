using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace Automation.Common;

/// <summary>
/// The Report instance during the run.
/// </summary>
public static class Report
{
    private static ExtentReports _extent;
    private static ExtentHtmlReporter _reporter;
    private static ExtentTest _currentTest;
    private static ExtentTest _infoNode;
    private static string _saveLocation;
    private static string _screenshotsFolder;

    public static string ScreenshotsFolder => _screenshotsFolder;

    /// <summary>
    /// Initializes the Report.
    /// </summary>
    static Report()
    {
        var reportsFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\Reports"));
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HH.mm.ss");
        _saveLocation = Path.Combine(reportsFolder, timestamp, "Results");
        _screenshotsFolder = Path.GetFullPath(Path.Combine(_saveLocation, "Screenshots"));
        if (!Directory.Exists(_saveLocation)) Directory.CreateDirectory(_saveLocation);
        if (!Directory.Exists(_screenshotsFolder)) Directory.CreateDirectory(_screenshotsFolder);
        _reporter = new ExtentHtmlReporter(_saveLocation);
        _reporter.Config.DocumentTitle = $"Test Run {DateTime.Now:yyMMdd}";
        _reporter.Config.ReportName = "Automation Report";            
        _extent = new ExtentReports();
        _extent.AttachReporter(_reporter);
    }

    /// <summary>
    /// Saves the Report to its folder.
    /// </summary>
    public static void Save()
    {
        _extent.Flush();
        Console.WriteLine("Report saved to " +  _saveLocation);
    }

    /// <summary>
    /// Creates a new test case in the Report.
    /// </summary>
    /// <param name="testName">The test name.</param>
    /// <exception cref="NullReferenceException">Thrown when the Report was not initialized.</exception>
    public static void NewTest(string testName)
    {
        _currentTest = _extent.CreateTest(testName);
    }

    /// <summary>
    /// Logs a test failure with an attached screenshot.
    /// </summary>
    /// <param name="error_message">The message to show in the log.</param>
    /// <param name="encodedByte64Screenshot">A screenshot in Base64 format.</param>
    public static void FailTest(string error_message, string encodedByte64Screenshot)
    {
        _currentTest.Fail("Test failed.").Fail(error_message, MediaEntityBuilder.CreateScreenCaptureFromBase64String(encodedByte64Screenshot).Build());
    }

    /// <summary>
    /// Logs a test pass with an attached screenshot.
    /// </summary>
    /// <param name="success_message">The message to show in the log.</param>
    /// <param name="encodedByte64Screenshot">A screenshot in Base64 format.</param>
    public static void PassTest(string success_message, string encodedByte64Screenshot)
    {
        _currentTest.Pass("Test passed!").Pass(success_message, MediaEntityBuilder.CreateScreenCaptureFromBase64String(encodedByte64Screenshot).Build());
    }

    /// <summary>
    /// Log a successful step with a screenshot.
    /// </summary>
    /// <param name="text">The log message.</param>
    /// <param name="encodedByte64Screenshot">A screenshot in Base64 format.</param>
    public static void PassAction(string text, string encodedByte64Screenshot)
    {
        _currentTest.Pass(text, MediaEntityBuilder.CreateScreenCaptureFromBase64String(encodedByte64Screenshot).Build());
    }

    /// <summary>
    /// Log a successful step.
    /// </summary>
    /// <param name="text">The log message.</param>
    public static void PassAction(string text)
    {
        _currentTest.Pass(text);
    }

    /// <summary>
    /// Log a failed step with a screenshot.
    /// </summary>
    /// <param name="text">The log message.</param>
    /// <param name="encodedByte64Screenshot">A screenshot in Base64 format.</param>
    public static void FailAction(string text, string encodedByte64Screenshot)
    {
        _currentTest.Fail(text, MediaEntityBuilder.CreateScreenCaptureFromBase64String(encodedByte64Screenshot).Build());
    }

    /// <summary>
    /// Log a failed step.
    /// </summary>
    /// <param name="text">The log message.</param>
    public static void FailAction(string text)
    {
        _currentTest.Fail(text);
    }

    /// <summary>
    /// Log a failed step from an Exception object with an attached screenshot.
    /// </summary>
    /// <param name="ex">Any exception object.</param>
    /// <param name="encodedByte64Screenshot">A screenshot in Base64 format.</param>
    public static void FailAction(Exception ex, string encodedByte64Screenshot)
    {
        _currentTest.Fail(ex, MediaEntityBuilder.CreateScreenCaptureFromBase64String(encodedByte64Screenshot).Build());
    }

    /// <summary>
    /// Log a failed step from an Exception object with an attached screenshot.
    /// </summary>
    /// <param name="ex">Any exception object.</param>
    public static void FailAction(Exception ex)
    {
        _currentTest.Fail(ex);
    }

    /// <summary>
    /// Log a generic message with no specific outcome.
    /// </summary>
    /// <param name="message"></param>
    public static void LogInfo(string message)
    {
        _currentTest.Info(message);
    }

    /// <summary>
    /// Log multiple lines to a separate node with any necessary test information.
    /// </summary>
    /// <param name="info">All lines to log.</param>
    public static void LogTestInfo(string[] info)
    {
        _infoNode ??= _currentTest.CreateNode("Test info");
        foreach (var item in info) _infoNode.Info(item);
    }

    /// <summary>
    /// Log a single line to a separate node with any necessary test information.
    /// </summary>
    /// <param name="info">A line to log.</param>
    public static void LogTestInfo(string info)
    {
        _infoNode ??= _currentTest.CreateNode("Test info");
        _infoNode.Info(info);
    }
}
