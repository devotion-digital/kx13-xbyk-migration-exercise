using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using TW.Web.Widgets.CallToAction;

[assembly: RegisterWidget(
    identifier: CallToActionWidgetViewComponent.Identifier,
    viewComponentType: typeof(CallToActionWidgetViewComponent),
    name: "Call to action",
    propertiesType: typeof(CallToActionWidgetProperties),
    Description = "A heading + supporting text + button that links to a page.",
    IconClass = "icon-cogwheel-square")]

namespace TW.Web.Widgets.CallToAction;

public class CallToActionWidgetViewComponent : ViewComponent
{
    // Matches K13 identifier so migrated widget data binds correctly.
    public const string Identifier = "TestWebsite.CallToAction";

    private readonly IContentRetriever contentRetriever;

    public CallToActionWidgetViewComponent(IContentRetriever contentRetriever)
    {
        this.contentRetriever = contentRetriever;
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<CallToActionWidgetProperties> componentViewModel)
    {
        var props = componentViewModel.Properties;

        string? buttonUrl = null;
        var selection = props.ButtonPage?.FirstOrDefault();
        if (selection != null && selection.WebPageGuid != System.Guid.Empty)
        {
            var pages = await contentRetriever.RetrieveAllPagesByGuids<IWebPageFieldsSource>(
                new[] { selection.WebPageGuid });
            var page = pages.FirstOrDefault();
            buttonUrl = page?.GetUrl().RelativePath;
        }

        var model = new CallToActionWidgetViewModel
        {
            Heading = props.Heading,
            SupportingText = props.SupportingText,
            ButtonText = props.ButtonText,
            ButtonUrl = buttonUrl,
        };

        return View("~/Widgets/CallToAction/_CallToActionWidget.cshtml", model);
    }
}
