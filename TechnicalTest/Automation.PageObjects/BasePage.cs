using Automation.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automation.PageObjects;

/// <summary>
/// The base class that all page objects inherit from.
/// </summary>
public abstract class BasePage : BaseElement
{
    /// <summary>
    /// Abstract class that represents a page.
    /// </summary>
    /// <param name="driver">A driver instance.</param>
    public BasePage(IWebDriver driver) : base(driver) { }

    public virtual bool WaitForPageLoad()
    {
        return Wait.Until(d => !d.FindElement(By.XPath("//body")).GetAttribute("class").Contains(""));
    }

    /// <summary>
    /// Perform a JavaScript scroll to the target element
    /// </summary>
    /// <param name="element">Target Element</param>
    public void ScrollElementIntoView(IWebElement element)
    {
        var jsExec = (IJavaScriptExecutor)Driver;
        jsExec.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        Thread.Sleep(1000);
    }

    /// <summary>
    /// Scroll the window on the Y axis.
    /// (+) Positive values scroll down 
    /// (-) Negative values scroll up.
    /// </summary>
    /// <param name="pixels">Amount of pixels.</param>
    public void ScrollPage(int pixels)
    {
        var jsExec = (IJavaScriptExecutor)Driver;
        jsExec.ExecuteScript($"window.scrollBy(0,{pixels})");
        Thread.Sleep(1000);
    }
}
