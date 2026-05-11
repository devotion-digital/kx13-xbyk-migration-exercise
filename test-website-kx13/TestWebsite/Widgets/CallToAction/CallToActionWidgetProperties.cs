using System.Collections.Generic;
using System.Linq;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace TestWebsite.Widgets.CallToAction
{
    public class CallToActionWidgetProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Heading", Order = 0)]
        public string Heading { get; set; } = string.Empty;

        [EditingComponent(TextAreaComponent.IDENTIFIER, Label = "Supporting text", Order = 1)]
        public string SupportingText { get; set; } = string.Empty;

        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Button text", Order = 2)]
        public string ButtonText { get; set; } = string.Empty;

        [EditingComponent(PageSelector.IDENTIFIER, Label = "Button target page", Order = 3)]
        [EditingComponentProperty(nameof(PageSelectorProperties.MaxPagesLimit), 1)]
        public IEnumerable<PageSelectorItem> ButtonPage { get; set; } = Enumerable.Empty<PageSelectorItem>();
    }
}
