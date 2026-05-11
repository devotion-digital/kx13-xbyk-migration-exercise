using System.Collections.Generic;

using CMS.Websites;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using Kentico.PageBuilder.Web.Mvc;

namespace TW.Web.Widgets.ChildPages;

public class ChildPagesWidgetProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Heading", Order = 10)]
    public string Heading { get; set; } = string.Empty;

    [WebPageSelectorComponent(Label = "Parent page (defaults to current page)", Order = 20, MaximumPages = 1)]
    public IEnumerable<WebPageRelatedItem> ParentPage { get; set; } = new List<WebPageRelatedItem>();

    [NumberInputComponent(Label = "Max tiles", Order = 30)]
    public int MaxTiles { get; set; } = 12;
}
