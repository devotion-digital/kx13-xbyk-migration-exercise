using System.Collections.Generic;
using System.Linq;

using CMS.ContentEngine;

using TW.Web.Generated;

namespace TW.Web.Services;

/// <summary>
/// Resolves URLs from migrated <see cref="MediaFile"/> linked items (the legacy media files
/// produced by the K13 → XbyK migration tool).
/// </summary>
public static class MediaHelper
{
    public static string? FirstUrl(IEnumerable<IContentItemFieldsSource>? items) =>
        items?.OfType<MediaFile>().FirstOrDefault()?.LegacyMediaFileAsset?.Url;
}
