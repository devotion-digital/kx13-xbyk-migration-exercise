using System.Collections.Generic;

namespace TW.Web.Widgets.ChildPages;

public class ChildPagesWidgetViewModel
{
    public string Heading { get; set; } = string.Empty;
    public IReadOnlyList<ChildPageTileViewModel> Tiles { get; set; } = new List<ChildPageTileViewModel>();
}

public class ChildPageTileViewModel
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
}
