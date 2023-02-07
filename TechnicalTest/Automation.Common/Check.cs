using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Common;

/// <summary>
/// Wrapper class for NUnit.Assert. Performs assertions and logs them in the Report.
/// </summary>
public static class Check
{
    /// <summary>
    /// Assert a given condition or constraint and log the result in the report with an attached screenshot.
    /// </summary>
    /// <param name="condition">Any function returning a boolean.</param>
    /// <param name="success_message">Message to print to report if assert succeeds.</param>
    /// <param name="error_message">Message to print to report if assert fails.</param>
    /// <param name="base64Screenshot">A screenshot in Base64 string format.</param>
    public static void That(bool condition, string success_message, string error_message, string base64Screenshot)
    {
        try
        {
            Assert.That(condition, error_message);
            Report.PassAction(success_message, base64Screenshot);
        }
        catch (AssertionException ex)
        {
            Report.FailAction(error_message + Environment.NewLine + ex, base64Screenshot);
            throw ex;
        }
    }

    /// <summary>
    /// Assert a given condition or constraint and log the result in the report with an attached screenshot.
    /// </summary>
    /// <typeparam name="TActual"></typeparam>
    /// <param name="actual">A value to check against the constraint.</param>
    /// <param name="constraint">The constraint to check the value against.</param>
    /// <param name="success_message">Message to print in report if assert succeeds.</param>
    /// <param name="base64Screenshot">A screenshot in Base64 string format.</param>
    public static void That<TActual>(TActual actual, IConstraint constraint, string success_message, string base64Screenshot)
    {
        try
        {
            Assert.That(actual, constraint);
            Report.PassAction(success_message, base64Screenshot);
        }
        catch (AssertionException ex)
        {
            Report.FailAction(ex, base64Screenshot);
            throw ex;
        }
    }

    /// <summary>
    /// Assert a given condition or constraint and log the result in the report with an attached screenshot.
    /// </summary>
    /// <param name="condition">Any function or expression returning a boolean.</param>
    /// <param name="base64Screenshot">A screenshot in Base64 string format.</param>
    public static void That(bool condition, string base64Screenshot)
    {
        try
        {
            Assert.That(condition);
            Report.PassAction("Action passed.", base64Screenshot);
        }
        catch (AssertionException ex)
        {
            Report.FailAction(ex, base64Screenshot);
            throw ex;
        }
    }
}
