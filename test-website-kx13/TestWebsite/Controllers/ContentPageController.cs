using CMS.DocumentEngine.Types.TW;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using TestWebsite.Controllers;
using TestWebsite.Models;

[assembly: RegisterPageRoute(ContentPage.CLASS_NAME, typeof(ContentPageController))]

namespace TestWebsite.Controllers
{
    public class ContentPageController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;

        public ContentPageController(IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
        }

        public IActionResult Index()
        {
            var page = pageDataContextRetriever.Retrieve<ContentPage>().Page;

            var model = new ContentPageViewModel
            {
                Name = page.ContentPageName,
                Subheading = page.ContentPageSubheading,
                MetaTitle = page.PageMetaTitle,
                MetaDescription = page.PageMetaDescription,
            };

            return View(model);
        }
    }
}
