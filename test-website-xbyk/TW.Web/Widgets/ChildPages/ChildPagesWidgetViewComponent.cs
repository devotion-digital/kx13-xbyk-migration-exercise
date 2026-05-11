using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using TW.Web.Generated;
using TW.Web.Widgets.ChildPages;

[assembly: RegisterWidget(
    identifier: ChildPagesWidgetViewComponent.Identifier,
    viewComponentType: typeof(ChildPagesWidgetViewComponent),
    name: "Child pages",
    propertiesType: typeof(ChildPagesWidgetProperties),
    Description = "Lists Content Pages that are children of the selected page as tiles.",
    IconClass = "icon-app-grid")]

namespace TW.Web.Widgets.ChildPages;

public class ChildPagesWidgetViewComponent : ViewComponent
{
    // Matches K13 identifier so migrated widget data binds correctly.
    public const string Identifier = "TestWebsite.ChildPages";

    private readonly IContentRetriever contentRetriever;

    public ChildPagesWidgetViewComponent(IContentRetriever contentRetriever)
    {
        this.contentRetriever = contentRetriever;
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<ChildPagesWidgetProperties> componentViewModel)
    {
        var props = componentViewModel.Properties;
        string parentPath = await ResolveParentPath(props);

        var children = await contentRetriever.RetrievePages<ContentPage>(
            new RetrievePagesParameters
            {
                PathMatch = PathMatch.Children(parentPath, nestingLevel: 1),
            });

        var tiles = children
            .Take(props.MaxTiles)
            .Select(child => new ChildPageTileViewModel
            {
                Title = string.IsNullOrWhiteSpace(child.ContentPageName) ? child.DocumentName : child.ContentPageName,
                Description = child.ContentPageSubheading,
                Url = child.GetUrl().RelativePath,
            })
            .ToList();

        var model = new ChildPagesWidgetViewModel
        {
            Heading = props.Heading,
            Tiles = tiles,
        };

        return View("~/Widgets/ChildPages/_ChildPagesWidget.cshtml", model);
    }

    private async Task<string> ResolveParentPath(ChildPagesWidgetProperties props)
    {
        // Explicit selection takes precedence.
        var selection = props.ParentPage?.FirstOrDefault();
        if (selection != null && selection.WebPageGuid != System.Guid.Empty)
        {
            var pages = await contentRetriever.RetrieveAllPagesByGuids<IWebPageFieldsSource>(
                new[] { selection.WebPageGuid });
            return pages.FirstOrDefault()?.SystemFields.WebPageItemTreePath ?? "/";
        }

        // Otherwise default to the current page the widget is placed on.
        var currentContent = await contentRetriever.RetrieveCurrentPage<ContentPage>();
        if (currentContent != null)
        {
            return currentContent.SystemFields.WebPageItemTreePath;
        }

        var currentHome = await contentRetriever.RetrieveCurrentPage<HomePage>();
        if (currentHome != null)
        {
            return currentHome.SystemFields.WebPageItemTreePath;
        }

        return "/";
    }
}
