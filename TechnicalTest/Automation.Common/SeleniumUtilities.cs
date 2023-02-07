using OpenQA.Selenium.Support.Extensions;
using System.Reflection;

namespace Automation.Common;

/// <summary>
/// Provides Selenium custom extension methods.
/// </summary>
public static class SeleniumUtilities
{
    public static bool IsChecked(this IWebElement checkbox)
    {
        return checkbox.GetAttribute("checked") == "true";
    }

    public static bool IsTextboxFilled(this IWebElement textbox)
    {
        return textbox.GetAttribute("value") != "";
    }

    public static bool IsActive(this IWebElement element)
    {
        return element.GetAttribute("class").Contains("active");
    }
}
