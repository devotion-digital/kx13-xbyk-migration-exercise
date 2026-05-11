using System.Collections.Generic;

using CMS.Websites;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Websites.FormAnnotations;
using Kentico.PageBuilder.Web.Mvc;

namespace TW.Web.Widgets.CallToAction;

public class CallToActionWidgetProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Heading", Order = 10)]
    public string Heading { get; set; } = string.Empty;

    [TextAreaComponent(Label = "Supporting text", Order = 20)]
    public string SupportingText { get; set; } = string.Empty;

    [TextInputComponent(Label = "Button text", Order = 30)]
    public string ButtonText { get; set; } = string.Empty;

    [WebPageSelectorComponent(Label = "Button target page", Order = 40, MaximumPages = 1)]
    public IEnumerable<WebPageRelatedItem> ButtonPage { get; set; } = new List<WebPageRelatedItem>();
}
