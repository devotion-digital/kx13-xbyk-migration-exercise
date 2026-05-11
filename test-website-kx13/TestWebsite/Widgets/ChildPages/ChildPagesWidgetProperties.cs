using System.Collections.Generic;
using System.Linq;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace TestWebsite.Widgets.ChildPages
{
    public class ChildPagesWidgetProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Heading", Order = 0)]
        public string Heading { get; set; } = string.Empty;

        [EditingComponent(PageSelector.IDENTIFIER, Label = "Parent page (defaults to current page)", Order = 1)]
        [EditingComponentProperty(nameof(PageSelectorProperties.MaxPagesLimit), 1)]
        public IEnumerable<PageSelectorItem> ParentPage { get; set; } = Enumerable.Empty<PageSelectorItem>();

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "Max tiles", Order = 2)]
        public int MaxTiles { get; set; } = 12;
    }
}
