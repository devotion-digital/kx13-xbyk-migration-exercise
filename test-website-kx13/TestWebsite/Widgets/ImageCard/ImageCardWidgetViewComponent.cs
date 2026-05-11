using System.Linq;

using CMS.MediaLibrary;
using CMS.SiteProvider;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using TestWebsite.Widgets.ImageCard;

[assembly: RegisterWidget(
    identifier: ImageCardWidgetViewComponent.Identifier,
    viewComponentType: typeof(ImageCardWidgetViewComponent),
    name: "Image card",
    propertiesType: typeof(ImageCardWidgetProperties),
    Description = "An image (from the media library) with a caption.",
    IconClass = "icon-picture")]

namespace TestWebsite.Widgets.ImageCard
{
    public class ImageCardWidgetViewComponent : ViewComponent
    {
        public const string Identifier = "TestWebsite.ImageCard";

        private readonly IMediaFileInfoProvider mediaFileInfoProvider;
        private readonly IMediaFileUrlRetriever mediaFileUrlRetriever;

        public ImageCardWidgetViewComponent(
            IMediaFileInfoProvider mediaFileInfoProvider,
            IMediaFileUrlRetriever mediaFileUrlRetriever)
        {
            this.mediaFileInfoProvider = mediaFileInfoProvider;
            this.mediaFileUrlRetriever = mediaFileUrlRetriever;
        }

        public IViewComponentResult Invoke(ComponentViewModel<ImageCardWidgetProperties> componentViewModel)
        {
            var props = componentViewModel.Properties;

            var model = new ImageCardWidgetViewModel
            {
                ImageUrl = ResolveImageUrl(props),
                AltText = props.AltText,
                Caption = props.Caption,
            };

            return View("~/Widgets/ImageCard/_ImageCardWidget.cshtml", model);
        }

        private string? ResolveImageUrl(ImageCardWidgetProperties props)
        {
            var selection = props.Image?.FirstOrDefault();
            if (selection == null)
            {
                return null;
            }

            var mediaFile = mediaFileInfoProvider.Get(selection.FileGuid, SiteContext.CurrentSiteID);
            return mediaFile == null ? null : mediaFileUrlRetriever.Retrieve(mediaFile).RelativePath;
        }
    }
}
