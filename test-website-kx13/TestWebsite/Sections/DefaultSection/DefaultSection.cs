using Kentico.PageBuilder.Web.Mvc;

using TestWebsite.Sections.DefaultSection;

[assembly: RegisterSection(
    identifier: DefaultSection.Identifier,
    name: "Default section",
    customViewName: "~/Sections/DefaultSection/_DefaultSection.cshtml",
    Description = "Single-column section that hosts widgets.",
    IconClass = "icon-square")]

namespace TestWebsite.Sections.DefaultSection
{
    public static class DefaultSection
    {
        public const string Identifier = "TestWebsite.DefaultSection";
    }
}
