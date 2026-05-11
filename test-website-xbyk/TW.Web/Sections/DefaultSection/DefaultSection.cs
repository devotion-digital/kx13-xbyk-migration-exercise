using Kentico.PageBuilder.Web.Mvc;

using TW.Web.Sections.DefaultSection;

[assembly: RegisterSection(
    identifier: DefaultSection.Identifier,
    name: "Default section",
    customViewName: "~/Sections/DefaultSection/_DefaultSection.cshtml",
    Description = "Single-column section that hosts widgets.",
    IconClass = "icon-square")]

namespace TW.Web.Sections.DefaultSection;

public static class DefaultSection
{
    // Matches the section identifier used by the K13 delivery site so migrated widget data
    // (which references "TestWebsite.DefaultSection") finds a registered section here.
    public const string Identifier = "TestWebsite.DefaultSection";
}
