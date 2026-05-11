using System.Linq;

using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using TestWebsite.Widgets.CallToAction;

[assembly: RegisterWidget(
    identifier: CallToActionWidgetViewComponent.Identifier,
    viewComponentType: typeof(CallToActionWidgetViewComponent),
    name: "Call to action",
    propertiesType: typeof(CallToActionWidgetProperties),
    Description = "A heading + supporting text + button that links to a page.",
    IconClass = "icon-cogwheel-square")]

namespace TestWebsite.Widgets.CallToAction
{
    public class CallToActionWidgetViewComponent : ViewComponent
    {
        public const string Identifier = "TestWebsite.CallToAction";

        private readonly IPageRetriever pageRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public CallToActionWidgetViewComponent(IPageRetriever pageRetriever, IPageUrlRetriever pageUrlRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public IViewComponentResult Invoke(ComponentViewModel<CallToActionWidgetProperties> componentViewModel)
        {
            var props = componentViewModel.Properties;

            var model = new CallToActionWidgetViewModel
            {
                Heading = props.Heading,
                SupportingText = props.SupportingText,
                ButtonText = props.ButtonText,
                ButtonUrl = ResolveButtonUrl(props),
            };

            return View("~/Widgets/CallToAction/_CallToActionWidget.cshtml", model);
        }

        private string? ResolveButtonUrl(CallToActionWidgetProperties props)
        {
            var selection = props.ButtonPage?.FirstOrDefault();
            if (selection == null)
            {
                return null;
            }

            var page = pageRetriever.Retrieve<TreeNode>(q => q.WhereEquals("NodeGUID", selection.NodeGuid)).FirstOrDefault();
            return page == null ? null : pageUrlRetriever.Retrieve(page).RelativePath;
        }
    }
}
