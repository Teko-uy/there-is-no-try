using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automation.PageObjects.Pages;

public class MLResultsPage : BasePage
{
    private const string _resultsQuantity = "//div[contains(@class,'ui-search-main')]//span[contains(@class,'quantity-results')]";
    private const string _searchResult = "//section[contains(@class,'ui-search-results')]//div[contains(@class, 'ui-search-result__content-wrapper')]";
    private const string _s_productTitle = ".//div[contains(@class,'group--title')]//a";
    private const string _s_productCurrency = ".//span[contains(@class,'andes-money-amount')]/span[contains(@class,'currency-symbol')]";
    private const string _s_productPrice = ".//span[contains(@class,'andes-money-amount')]/span[contains(@class,'fraction')]";
    private const string _pageCount = "//li[contains(@class,'page-count')]";
    private const string _nextPage = "//li[contains(@class,'button--next')]/a";

    public IWebElement ResultsQuantity => FindVisible(_resultsQuantity);
    public IWebElement PageCount => FindVisible(_pageCount);
    public IWebElement NextPage => FindVisible(_nextPage);
    public IReadOnlyCollection<IWebElement> SearchResults => FindAllEnabled(_searchResult);

    protected int GetPageCount()
    {
        string quantity = Regex.Match(PageCount.Text, @"[0-9]").Value;
        return int.Parse(quantity);
    }
    public List<string> GetProductsFromPages(int pages)
    {
        if(pages > GetPageCount()) pages = GetPageCount();
        List<string> products = new() { "Nombre,Moneda,Precio,Link" };
        for (int i = 1; i <= pages; i++)
        {
            if (i > 1)
            {
                ScrollToBottom();
                NextPage.Click();
            }
            foreach (IWebElement result in SearchResults)
            {
                string title = result.FindElement(By.XPath(_s_productTitle)).GetAttribute("title");
                string currency = result.FindElement(By.XPath(_s_productCurrency)).Text;
                string price = result.FindElement(By.XPath(_s_productPrice)).Text;
                string link = result.FindElement(By.XPath(_s_productTitle)).GetAttribute("href");
                products.Add($"{title},{currency},{price},{link}");
            }
        }
        return products;
    }

    public MLResultsPage(IWebDriver driver) : base(driver) { }

}
