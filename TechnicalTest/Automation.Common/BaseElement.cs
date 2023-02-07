using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Automation.Common;

/// <summary>
/// Base class for every web component or page in the framework. Holds methods for finding web elements using waits.
/// </summary>
public abstract class BaseElement
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    /// <summary>
    /// Exposes the current driver instance.
    /// </summary>
    protected IWebDriver Driver => _driver;
    /// <summary>
    /// Exposes the current wait instance.
    /// </summary>
    protected WebDriverWait Wait => _wait;

    /// <summary>
    /// Base constructor.
    /// </summary>
    /// <param name="driver">Driver instance.</param>
    public BaseElement(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));
    }

    /// <summary>
    /// Waits for an element to exist in the DOM and returns it.
    /// </summary>
    /// <param name="xpath">The element's xpath.</param>
    /// <returns></returns>
    protected IWebElement Find(string xpath)
    {
        _wait.Until(d => 
        {
            IWebElement e = d.FindElement(By.XPath(xpath));
            return e;
        });
        return _driver.FindElement(By.XPath(xpath));
    }

    /// <summary>
    /// Waits for an element to exist and be visible in the DOM and returns it.
    /// </summary>
    /// <param name="xpath">The element's xpath.</param>
    /// <returns></returns>
    protected IWebElement FindVisible(string xpath)
    {
        _wait.Until(d =>
        {
            IWebElement e = d.FindElement(By.XPath(xpath));
            return e.Displayed;
        });
        return _driver.FindElement(By.XPath(xpath));
    }

    /// <summary>
    /// Waits for an element to exist and be visible in the DOM and returns it.
    /// </summary>
    /// <param name="xpath">The base xpath with string formatting.</param>
    /// <param name="tokens">Tokens to complete the base xpath</param>
    /// <returns></returns>
    protected IWebElement FindVisible(string xpath, params string[] tokens)
    {
        string formattedXPath = string.Format(xpath, tokens);
        _wait.Until(d =>
        {
            IWebElement e = d.FindElement(By.XPath(formattedXPath));
            return e.Displayed;
        });
        return _driver.FindElement(By.XPath(formattedXPath));
    }

    /// <summary>
    /// Waits for an element to exist and be enabled in the DOM and returns it.
    /// </summary>
    /// <param name="xpath">The element's xpath.</param>
    /// <returns></returns>
    protected IWebElement FindEnabled(string xpath)
    {
        _wait.Until(d =>
        {
            IWebElement e = d.FindElement(By.XPath(xpath));
            return e.Enabled;
        });
        return _driver.FindElement(By.XPath(xpath));
    }

    /// <summary>
    /// Waits for an element to exist, scrolls the page until it is visible and returns it.
    /// </summary>
    /// <param name="xpath">The element's xpath.</param>
    /// <returns></returns>
    protected IWebElement ScrollFind(string xpath, params string[] tokens)
    {
        string formattedXPath = string.Format(xpath, tokens);
        _wait.Until(d =>
        {
            IWebElement e = d.FindElement(By.XPath(formattedXPath));
            Actions a = new(d);
            a.MoveToElement(e).Build().Perform();
            return e.Displayed;
        });
        return _driver.FindElement(By.XPath(formattedXPath));
    }

    /// <summary>
    /// Finds all elements that match the xpath expression and returns them as a readonly list.
    /// </summary>
    /// <param name="xpath">The elements' xpath.</param>
    /// <returns></returns>
    protected IReadOnlyCollection<IWebElement> FindAll(string xpath)
    {
        return _driver.FindElements(By.XPath(xpath)).ToList().AsReadOnly();
    }

    /// <summary>
    /// Finds all visible elements that match the xpath expression and returns them as a readonly list.
    /// </summary>
    /// <param name="xpath">The elements' xpath.</param>
    /// <returns></returns>
    protected IReadOnlyCollection<IWebElement> FindAllVisible(string xpath)
    {
        return _driver.FindElements(By.XPath(xpath)).Where(e => e.Displayed).ToList().AsReadOnly();
    }

    /// <summary>
    /// Finds all enabled elements that match the xpath expression and returns them as a readonly list.
    /// </summary>
    /// <param name="xpath">The elements' xpath.</param>
    /// <returns></returns>
    protected IReadOnlyCollection<IWebElement> FindAllEnabled(string xpath)
    {
        return _driver.FindElements(By.XPath(xpath)).Where(e => e.Enabled).ToList().AsReadOnly();
    }

    /// <summary>
    /// Waits for an element to have a specific value for a given css property.
    /// </summary>
    /// <param name="locator">The element's xpath.</param>
    /// <param name="property">The css property to poll for.</param>
    /// <param name="expectedValue">The expected value.</param>
    public bool WaitForCssProperty(string locator, string property, string expectedValue)
    {
        return _wait.Until(d =>
        {
            return d.FindElement(By.XPath(locator)).GetCssValue(property) == expectedValue;
        });
    }

    /// <summary>
    /// Converts a generic element to a SelectElement and selects an option using the provided text.
    /// </summary>
    /// <param name="xpath">The element's xpath.</param>
    /// <param name="text">The option's text.</param>
    public void SelectByText(string xpath, string text)
    {
        var selectElement = new SelectElement(FindVisible(xpath));
        selectElement.SelectByText(text);
    }

    /// <summary>
    /// Converts a generic element to a SelectElement and selects an option by index.
    /// </summary>
    /// <param name="xpath">The element's xpath.</param>
    /// <param name="index">The option's index.</param>
    public void SelectByIndex(string xpath, int index)
    {
        var selectElement = new SelectElement(FindVisible(xpath));
        selectElement.SelectByIndex(index);
    }
}
