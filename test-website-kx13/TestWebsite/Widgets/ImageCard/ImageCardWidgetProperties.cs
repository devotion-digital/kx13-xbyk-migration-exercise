using System.Collections.Generic;
using System.Linq;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace TestWebsite.Widgets.ImageCard
{
    public class ImageCardWidgetProperties : IWidgetProperties
    {
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Label = "Image", Order = 0)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        public IEnumerable<MediaFilesSelectorItem> Image { get; set; } = Enumerable.Empty<MediaFilesSelectorItem>();

        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Alt text", Order = 1)]
        public string AltText { get; set; } = string.Empty;

        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Caption", Order = 2)]
        public string Caption { get; set; } = string.Empty;
    }
}
