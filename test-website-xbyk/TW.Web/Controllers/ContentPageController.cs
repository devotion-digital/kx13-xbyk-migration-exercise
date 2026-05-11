using System.Threading.Tasks;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using TW.Web.Controllers;
using TW.Web.Generated;
using TW.Web.Models;

[assembly: RegisterWebPageRoute(
    contentTypeName: ContentPage.CONTENT_TYPE_NAME,
    controllerType: typeof(ContentPageController))]

namespace TW.Web.Controllers;

public class ContentPageController : Controller
{
    private readonly IContentRetriever contentRetriever;

    public ContentPageController(IContentRetriever contentRetriever)
    {
        this.contentRetriever = contentRetriever;
    }

    public async Task<IActionResult> Index()
    {
        var page = await contentRetriever.RetrieveCurrentPage<ContentPage>(
            new RetrieveCurrentPageParameters());

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
