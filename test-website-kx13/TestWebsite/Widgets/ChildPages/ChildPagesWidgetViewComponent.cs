using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.TW;
using CMS.SiteProvider;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using TestWebsite.Widgets.ChildPages;

[assembly: RegisterWidget(
    identifier: ChildPagesWidgetViewComponent.Identifier,
    viewComponentType: typeof(ChildPagesWidgetViewComponent),
    name: "Child pages",
    propertiesType: typeof(ChildPagesWidgetProperties),
    Description = "Lists Content Pages that are children of the current (or selected) page as tiles.",
    IconClass = "icon-app-grid")]

namespace TestWebsite.Widgets.ChildPages
{
    public class ChildPagesWidgetViewComponent : ViewComponent
    {
        public const string Identifier = "TestWebsite.ChildPages";

        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageRetriever pageRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public ChildPagesWidgetViewComponent(
            IPageDataContextRetriever pageDataContextRetriever,
            IPageRetriever pageRetriever,
            IPageUrlRetriever pageUrlRetriever)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageRetriever = pageRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public IViewComponentResult Invoke(ComponentViewModel<ChildPagesWidgetProperties> componentViewModel)
        {
            var props = componentViewModel.Properties;
            var parent = ResolveParentPage(props);

            var tiles = parent == null
                ? new List<ChildPageTileViewModel>()
                : BuildTiles(parent, props.MaxTiles);

            var model = new ChildPagesWidgetViewModel
            {
                Heading = props.Heading,
                Tiles = tiles,
            };

            return View("~/Widgets/ChildPages/_ChildPagesWidget.cshtml", model);
        }

        private TreeNode? ResolveParentPage(ChildPagesWidgetProperties props)
        {
            var selection = props.ParentPage?.FirstOrDefault();
            if (selection != null)
            {
                return pageRetriever
                    .Retrieve<TreeNode>(q => q.WhereEquals("NodeGUID", selection.NodeGuid))
                    .FirstOrDefault();
            }

            return pageDataContextRetriever.TryRetrieve<TreeNode>(out var ctx) ? ctx.Page : null;
        }

        private IReadOnlyList<ChildPageTileViewModel> BuildTiles(TreeNode parent, int maxTiles)
        {
            var children = pageRetriever.Retrieve<ContentPage>(q => q
                .OnSite(SiteContext.CurrentSiteName)
                .Path(parent.NodeAliasPath, PathTypeEnum.Children)
                .NestingLevel(1)
                .OrderBy("NodeOrder")
                .TopN(maxTiles));

            return children.Select(page => new ChildPageTileViewModel
            {
                Title = string.IsNullOrWhiteSpace(page.ContentPageName) ? page.DocumentName : page.ContentPageName,
                Description = string.IsNullOrWhiteSpace(page.ContentPageSubheading) ? page.DocumentPageDescription : page.ContentPageSubheading,
                Url = pageUrlRetriever.Retrieve(page).RelativePath,
            }).ToList();
        }
    }
}
