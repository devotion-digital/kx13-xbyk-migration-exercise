using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

using Kentico.Content.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using TW.Web.Generated;

namespace TW.Web.Components.Navigation;

public class NavigationViewComponent : ViewComponent
{
    private readonly IContentRetriever contentRetriever;

    public NavigationViewComponent(IContentRetriever contentRetriever)
    {
        this.contentRetriever = contentRetriever;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var pages = await contentRetriever.RetrievePages<ContentPage>(
            new RetrievePagesParameters());

        // Filter to top-level only: tree path has exactly one slash, e.g. "/Services" (depth 1),
        // not "/Services/Service-A" (depth 2). The framework's PathMatch.Children(..., nestingLevel: 1)
        // against "/" was returning descendants too, so filter post-query.
        var items = pages
            .Where(p => IsTopLevel(p.SystemFields.WebPageItemTreePath))
            .OrderBy(p => p.SystemFields.WebPageItemOrder)
            .Select(p => new NavigationItemViewModel
            {
                Title = string.IsNullOrWhiteSpace(p.ContentPageName) ? p.DocumentName : p.ContentPageName,
                Url = p.GetUrl().RelativePath,
            })
            .ToList();

        return View((IReadOnlyList<NavigationItemViewModel>)items);
    }

    private static bool IsTopLevel(string? treePath) =>
        !string.IsNullOrEmpty(treePath) && treePath.Count(c => c == '/') == 1;
}
