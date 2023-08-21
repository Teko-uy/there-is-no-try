using Automation.PageObjects.Pages;
using Hms.Essette.GUI.TestsNew;
using NUnit.Framework;

namespace Automation.Tests.Scripts;

public class MLTests : BaseTest
{
    private MLHomePage _mlHomePage;
    private MLResultsPage _mlResultsPage;

    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();
        _mlHomePage = new MLHomePage(Driver);
        _mlResultsPage = new MLResultsPage(Driver);
    }

    [Test]
    public void GetTop3PagesForTShirts()
    {
        RunTest(() =>
        {
            _mlHomePage.SearchForProducts("camisetas");
            var results = _mlResultsPage.GetProductsFromPages(3);
            SaveProductDetails(results);
        });
    }
}
