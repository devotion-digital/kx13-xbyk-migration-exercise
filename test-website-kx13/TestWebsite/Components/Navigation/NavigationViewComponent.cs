using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.TW;
using CMS.SiteProvider;

using Kentico.Content.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace TestWebsite.Components.Navigation
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public NavigationViewComponent(IPageRetriever pageRetriever, IPageUrlRetriever pageUrlRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public IViewComponentResult Invoke()
        {
            var pages = pageRetriever.Retrieve<TreeNode>(q => q
                .OnSite(SiteContext.CurrentSiteName)
                .Path("/", PathTypeEnum.Children)
                .NestingLevel(1)
                .OrderBy("NodeOrder"));

            var items = pages
                .Where(p => p.ClassName != HomePage.CLASS_NAME)
                .Where(p => DataClassInfoProvider.GetDataClassInfo(p.ClassName)?.ClassHasURL == true)
                .Select(p => new NavigationItemViewModel
                {
                    Title = p.DocumentName,
                    Url = pageUrlRetriever.Retrieve(p).RelativePath,
                })
                .ToList();

            return View((IReadOnlyList<NavigationItemViewModel>)items);
        }
    }
}
