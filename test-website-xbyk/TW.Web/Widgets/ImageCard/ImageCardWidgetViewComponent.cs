using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using TW.Web.Generated;
using TW.Web.Widgets.ImageCard;

[assembly: RegisterWidget(
    identifier: ImageCardWidgetViewComponent.Identifier,
    viewComponentType: typeof(ImageCardWidgetViewComponent),
    name: "Image card",
    propertiesType: typeof(ImageCardWidgetProperties),
    Description = "An image (from the content hub) with a caption.",
    IconClass = "icon-picture")]

namespace TW.Web.Widgets.ImageCard;

public class ImageCardWidgetViewComponent : ViewComponent
{
    // Matches K13 identifier so migrated widget data binds correctly.
    public const string Identifier = "TestWebsite.ImageCard";

    private readonly IContentRetriever contentRetriever;

    public ImageCardWidgetViewComponent(IContentRetriever contentRetriever)
    {
        this.contentRetriever = contentRetriever;
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<ImageCardWidgetProperties> componentViewModel)
    {
        var props = componentViewModel.Properties;

        string? imageUrl = null;
        var selectedGuid = props.Image?.FirstOrDefault()?.Identifier;
        if (selectedGuid.HasValue && selectedGuid.Value != System.Guid.Empty)
        {
            var items = await contentRetriever.RetrieveContentByGuids<MediaFile>(
                new[] { selectedGuid.Value },
                new RetrieveContentParameters());
            imageUrl = items.FirstOrDefault()?.LegacyMediaFileAsset?.Url;
        }

        var model = new ImageCardWidgetViewModel
        {
            ImageUrl = imageUrl,
            AltText = props.AltText,
            Caption = props.Caption,
        };

        return View("~/Widgets/ImageCard/_ImageCardWidget.cshtml", model);
    }
}
