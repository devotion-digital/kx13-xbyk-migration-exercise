using System.Collections.Generic;

using CMS.ContentEngine;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.PageBuilder.Web.Mvc;

using TW.Web.Generated;

namespace TW.Web.Widgets.ImageCard;

public class ImageCardWidgetProperties : IWidgetProperties
{
    [ContentItemSelectorComponent(MediaFile.CONTENT_TYPE_NAME, Label = "Image", Order = 10, MaximumItems = 1)]
    public IEnumerable<ContentItemReference> Image { get; set; } = new List<ContentItemReference>();

    [TextInputComponent(Label = "Alt text", Order = 20)]
    public string AltText { get; set; } = string.Empty;

    [TextInputComponent(Label = "Caption", Order = 30)]
    public string Caption { get; set; } = string.Empty;
}
