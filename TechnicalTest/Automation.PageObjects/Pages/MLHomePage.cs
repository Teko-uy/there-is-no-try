using OpenQA.Selenium;

namespace Automation.PageObjects.Pages;

public class MLHomePage : BasePage
{
    public MLHomePage(IWebDriver driver) : base(driver) { }

    private readonly string _searchBox = "//header//input[@id='cb1-edit']";
    private readonly string _searchButton = "//header//button[@class='nav-search-btn']";

    public IWebElement SearchBox => FindVisible(_searchBox);
    public IWebElement SearchButton => FindVisible(_searchButton);

    public void SearchForProducts(string product)
    {
        SearchBox.SendKeys(product);
        SearchButton.Click();
    }
}
