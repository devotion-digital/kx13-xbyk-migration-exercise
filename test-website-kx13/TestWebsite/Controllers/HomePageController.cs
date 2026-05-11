using CMS.DocumentEngine.Types.TW;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using TestWebsite.Controllers;
using TestWebsite.Models;

[assembly: RegisterPageRoute(HomePage.CLASS_NAME, typeof(HomePageController))]

namespace TestWebsite.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;

        public HomePageController(IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
        }

        public IActionResult Index()
        {
            var page = pageDataContextRetriever.Retrieve<HomePage>().Page;

            var model = new HomePageViewModel
            {
                Name = page.HomePageName,
                Subheading = page.HomePageSubheading,
                HeroImageUrl = page.HomePageHeroImage,
                MetaTitle = page.PageMetaTitle,
                MetaDescription = page.PageMetaDescription,
            };

            return View(model);
        }
    }
}
